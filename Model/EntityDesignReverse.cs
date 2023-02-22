using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Model.Extensions
{
    /// <summary>
    /// If there is a class that implements IDesignTimeServices, 
    /// then the EF Tools will call it to allow custom services 
    /// to be registered.
    ///
    /// We implement this method so that we can replace some of 
    /// the services used during reverse engineer.
    /// 2018-05-24 By 齐岳，此类继承了IDesignTimeServices，并通过反射实现了依赖注入，Entityframework在执行脚手架脚本时会使用此类中重写的方法。
    /// 解决脚手架默认帕斯卡命名法的问题，改为全部大写
    /// </summary>
    /// 

    public class MyDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddSingleton<ICandidateNamingService, MyCandidateNamingService>();
            services.AddSingleton<ICSharpDbContextGenerator, MyDbContextGenerator>();
            //services.AddSingleton<ICSharpEntityTypeGenerator, MyCSharpEntityTypeGenerator>();
        }
    }

    /// <summary>
    /// 重写实体类属性命名方式，将原有的帕斯卡命名法改为通过大写的命名法
    /// </summary>
    public class MyCandidateNamingService : ICandidateNamingService
    {
        public string GenerateCandidateIdentifier(DatabaseTable originalTable)
        {
            return originalTable.Name.ToUpper();
        }

        public string GenerateCandidateIdentifier(DatabaseColumn originalColumn)
        {
            return originalColumn.Name.ToUpper();
        }

        public string GetDependentEndCandidateNavigationPropertyName(IForeignKey foreignKey)
        {
            return foreignKey.PrincipalEntityType.Name;
        }

        public string GetPrincipalEndCandidateNavigationPropertyName(IForeignKey foreignKey, string dependentEndNavigationPropertyName)
        {
            return dependentEndNavigationPropertyName;
        }
    }
    /// <summary>
    /// 实体文件内容
    /// </summary>
    public class MyCSharpEntityTypeGenerator : ICSharpEntityTypeGenerator
    {
        public string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            return null;
        }
    }
    //Context文件内容
    public class MyDbContextGenerator : CSharpDbContextGenerator
    {
        public MyDbContextGenerator(IEnumerable<IScaffoldingProviderCodeGenerator> a, IEnumerable<IProviderConfigurationCodeGenerator> b, IAnnotationCodeGenerator c, ICSharpHelper d) :
            base(a, b, c, d)
        { }
        /// <summary>
        /// 修改代码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="namespace"></param>
        /// <param name="contextName"></param>
        /// <param name="connectionString"></param>
        /// <param name="useDataAnnotations"></param>
        /// <param name="suppressConnectionStringWarning"></param>
        /// <returns></returns>
        public override string WriteCode(IModel model, string @namespace, string contextName, string connectionString, bool useDataAnnotations, bool suppressConnectionStringWarning)
        {
            var code = base.WriteCode(model, @namespace, contextName, connectionString, useDataAnnotations, suppressConnectionStringWarning);
            int start = code.IndexOf("public drugdbContext(Db");
            int end = code.IndexOf("public virtual");
            string re = code.Substring(start, end - start);
            code = "using Microsoft.AspNetCore.Http;\n" + code.Replace(re, "private readonly IHttpContextAccessor _httpContextAccessor;\n public drugdbContext(DbContextOptions<drugdbContext> options, IHttpContextAccessor httpContextAccessor=null): base(options){\n_httpContextAccessor = httpContextAccessor;\n}\n");
            return code;
        }
        /// <summary>
        /// 修改连接字符串配置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="suppressConnectionStringWarning"></param>
        protected override void GenerateOnConfiguring(string connectionString, bool suppressConnectionStringWarning)
        {
            //不写的话就不会生成onConfiguring相关代码
        }
    }
}