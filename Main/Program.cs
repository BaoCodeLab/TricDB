using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 150_000_000;//限制请求长度
            })
            .UseUrls("http://*:80")
            .UseStartup<Startup>();
    }
}