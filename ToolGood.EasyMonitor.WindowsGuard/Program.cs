using System;
using Topshelf;

namespace ToolGood.EasyMonitor.WindowsGuard
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.SetDisplayName("Windows监控守护程序");
                x.SetDescription("Windows监控守护程序");
                x.SetServiceName("ToolGood.EasyMonitor.WindowsGuard");

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
