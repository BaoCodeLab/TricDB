using Main.platform;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Main.Extensions
{
    public class AppConfigurtaionServices
    {
        /// <summary>
        /// 使用appsetting.json作为配置文件
        /// 读取数据库链接字符串：AppConfigurtaionServices.Configuration.GetConnectionString("CxyOrder"); 
        /// 读取一级配置节点配置：AppConfigurtaionServices.Configuration["ServiceUrl"];
        /// 读取二级子节点配置：AppConfigurtaionServices.Configuration["Appsettings:SystemName"];
        /// </summary>
        public static IConfiguration Configuration { get; set; }
        static AppConfigurtaionServices()
        {
            try
            {
                string root = string.Empty;
                try
                {
                     root = Directory.GetCurrentDirectory();
                    //DirectoryInfo Dir = Directory.GetParent(rootdir);
                    //root = Dir.Parent.Parent.FullName;
                }
                catch
                {
                    root = "D:\\CRM\\CRM_XZ";
                }
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(root)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings."+Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") +".json", optional: true, reloadOnChange: true)
                .Build();
            }
            catch (Exception ex)
            {
                Log.Write(typeof(string), "appsettings.json", ex.ToString());
            }
        }
    }
}
