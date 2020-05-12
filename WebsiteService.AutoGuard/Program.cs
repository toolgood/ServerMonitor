using System;
using Topshelf;

namespace WebsiteService.AutoGuard
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.SetDisplayName("守护程序");
                x.SetDescription("守护程序");
                x.SetServiceName("WebsiteService.AutoGuard");

                x.Service<MainService>(s =>
                {
                    s.ConstructUsing(name => new MainService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
            });
            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
