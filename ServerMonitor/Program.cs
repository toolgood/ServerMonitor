using ServerMonitor.Monitors;
using ServerMonitor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Topshelf;

namespace ServerMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x => {
                x.Service<MainService>(s => {
                    s.ConstructUsing(name => new MainService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("ServerMonitor");
                x.SetDisplayName("ServerMonitor");
                x.SetServiceName("ServerMonitor");
            });
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
