using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace ToolGood.EasyMonitor.WindowsGuard
{
    public class MainService
    {
        private readonly Timer _timer;
        public MainService()
        {
            _timer = new Timer(30 * 1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var scs = ServiceController.GetServices();
            var service = scs.FirstOrDefault(q => q.ServiceName == "ToolGood.EasyMonitor.WindowsClient");
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
            service = scs.FirstOrDefault(q => q.ServiceName == "ToolGood.EasyMonitor.WindowsServer");
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
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

    }

}
