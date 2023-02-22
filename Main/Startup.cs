using AutoMapper;
using Hangfire;
using Hangfire.MySql.Core;
using Main.platform;
using Main.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Model.Model;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TDSCoreLib;
using UEditorNetCore;
using System.Net.Http;

namespace Main
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets("secretID");
            }

            builder.AddEnvironmentVariables();
            Configuration = configuration;
            Environment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //解决mvc默认中文自动unicode的问题
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });


            //请求体长度限制
            services.Configure<FormOptions>(options =>
            {
                //options.MultipartBodyLengthLimit = 150_000_000;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });


            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //WebAPI返回json时会默认以小驼峰命名法来命名属性，下列配置取消了该设置
            services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
            //统一处理参数验证
            services.Configure<ApiBehaviorOptions>(a =>
            {
                a.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new CustomBadRequest(context);

                    return new BadRequestObjectResult(WebAPIErrorMsg.Failure(problemDetails.Detail))
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =(httpRequestMessage, cert, cetChain, policyErrors) =>{
                    return true;
                }
            });

            services.AddSession();
            //注册Hangfire实现任务定时调度 https://github.com/stulzq/Hangfire.MySql.Core
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(Configuration.GetConnectionString("MySQLConnection"))));
            //EF Frame上下文
            //注册EFDbContext，并在appsetting.json中读取数据库连接字符串
            services.AddEntityFrameworkMySql().AddDbContext<drugdbContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "API接口开放平台",
                    Version = "v1.0",
                    Description = "本平台用于API接口集中管理和查询使用，由武汉启辰合智科技有限公司©提供技术支持",
                    TermsOfService = "",
                    Contact = new Contact
                    {
                        Name = "",
                        Email = ""
                    },
                });
                //设置xml注释文档，注意名称一定要与项目名称相同
                //var filePath = Path.Combine(Environment.ContentRootPath, "TDSCore.xml");
                //c.IncludeXmlComments(filePath);
                //处理复杂名称
                c.CustomSchemaIds((type) => type.FullName);
            });
            #endregion
            //缓存
            services.AddMemoryCache();

            //保持机器码不变
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("./UNC-PATH"));
            //身份认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/login";
                    o.AccessDeniedPath = "/login";
                    o.ExpireTimeSpan = TimeSpan.FromDays(15);
                    o.Cookie.Expiration = TimeSpan.FromDays(15);
                    o.Cookie.HttpOnly = true;
                    o.Cookie.Name = "MyCookie";
                });


            services.AddHttpContextAccessor();
            //微信Senparc框架

            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册

            services.AddUEditorService();

            //修改默认的视图查找路径，以支持移动端公用cshtml
            services.AddMvc().AddRazorOptions(opt =>
            {
                opt.ViewLocationExpanders.Add(new ViewLocationExpander());
                opt.ViewLocationFormats.Add("/Views/Usercenter/{0}" + RazorViewEngine.ViewExtension);
                opt.ViewLocationFormats.Add("/Views/Website/{0}" + RazorViewEngine.ViewExtension);
            });
            //services.AddLocalization(opt => { opt.ResourcesPath = "Language"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<SenparcWeixinSetting> senparcWeixinSetting, IOptions<SenparcSetting> senparcSetting)
        {
            //注册用户信息
            Permission.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            var supportedCultures = new[]
            {
                new CultureInfo("en-US")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;//important
                cfg.CreateMap<DateTime, string>().ConvertUsing(new DateTimeTypeConverter());
                cfg.CreateMap<bool, string>().ConvertUsing(new BoolTypeConverter());
                cfg.CreateMap<VM_PF_PROFILE, PF_PROFILE>().ReverseMap();
                //菜单
                cfg.CreateMap<VM_PF_MENU, PF_MENU>().ReverseMap();
                //栏目
                cfg.CreateMap<VM_PF_LM, PF_LM>().ReverseMap();
                //文章
                cfg.CreateMap<VM_PF_NEW, PF_NEW>().ReverseMap();
                //定时提醒
                cfg.CreateMap<VM_PF_REMINDER, PF_REMINDER>().ReverseMap();
                //组织架构
                cfg.CreateMap<VM_PF_ORG, PF_ORG>().ReverseMap();
                cfg.CreateMap<VM_PF_USER_ORG, PF_USER_ORG>().ReverseMap();
                //表单授权
                cfg.CreateMap<VM_FD_FORM_PERMISSION, FD_FORM_PERMISSION>().ReverseMap();
                //模板管理
                cfg.CreateMap<VM_PF_PRINT_TMPL, PF_PRINT_TMPL>().ReverseMap();

            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //设置允许访问的文件类型
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider()
                {

                    Mappings = { [".properties"] = "text/html" }
                }
            });

            app.UseSession();
            app.UseAuthentication();
            //全局API异常监控中间件
            app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            //if (env.IsDevelopment())
            //{
            //API定时调度
            app.UseHangfireServer(new BackgroundJobServerOptions { });//启动Hangfire服务
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {

                Authorization = new[] { new HangfireAuthorizationFilter() }
            });//启动hangfire面板
            //}
            //  RecurringJob.AddOrUpdate(() => Console.WriteLine($"ASP.NET core Hangfire"), Cron.Minutely());
            #region SwaggerAPI平台
            app.UseSwagger(c =>
            {
                //设置json路径
                c.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                //访问swagger UI的路由，如http://localhost:端口/docs
                c.RoutePrefix = "docs";
                c.DocumentTitle = "API接口开放平台";
                //c.HeadContent = "TRG OPEN-API Platform";会显示在网页最顶端，可以是html
                c.SwaggerEndpoint("/docs/v1/swagger.json", "OPEN-API Platform V1.0");
                //更改UI样式
                c.InjectStylesheet("/swagger-ui/custom.css");
            });
            #endregion

            //日志管理
            loggerFactory.AddNLog();//添加NLog  
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件 

            //微信框架

            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();// 启动 CO2NET 全局注册，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);//微信全局注册，必须！
            app.UseEnableRequestRewind();
            //设置启动默认路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}