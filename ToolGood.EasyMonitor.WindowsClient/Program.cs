using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Topshelf;

namespace ToolGood.EasyMonitor.WindowsClient
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureLogging(q => q.SetMinimumLevel(LogLevel.None))
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        public static void Main(string[] args)
        {
            var rc = HostFactory.Run(x => {
                x.SetDisplayName("Windows监控客户端");
                x.SetDescription("Windows监控客户端");
                x.SetServiceName("ToolGood.EasyMonitor.WindowsClient");

                x.Service<MainService>(s => {
                    s.ConstructUsing(name => new MainService(CreateHostBuilder(args).Build()));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(async tc => await tc.Stop());
                });
                x.RunAsLocalSystem();
            });
            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;

         }

 
    }
}
