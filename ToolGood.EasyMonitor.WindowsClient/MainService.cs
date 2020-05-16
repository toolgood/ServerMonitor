using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;

namespace ToolGood.EasyMonitor.WindowsClient
{
    public class MainService
    {
        private readonly Timer _timer;
        private readonly IHost _host;
        public MainService(IHost host)
        {
            _host = host;
            _timer = new Timer(5 * 60 * 1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var scs = ServiceController.GetServices();
            var service = scs.FirstOrDefault(q => q.ServiceName == "ToolGood.EasyMonitor.WindowsGuard");
            if (service != null)
            {
                if (service.StartType != ServiceStartMode.Disabled)
                {
                    if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                    {
                        try
                        {
                            service.Start();
                        }
                        catch { }
                    }
                }
            }

            //var monitorInfo = MonitorUtil.GetMachineMonitorInfo();
            //var sites = MonitorUtil.GetSiteInfos();
        }
        public void Start()
        {
            Timer_Elapsed(_timer, null);
            _timer.Start();
            _host.Run();
        }
        public async Task Stop()
        {
            _timer.Stop();
            await _host.StopAsync();
        }

    }

}
