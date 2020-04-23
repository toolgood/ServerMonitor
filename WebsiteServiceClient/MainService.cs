using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace WebsiteServiceClient
{
    public class MainService
    {
        private readonly Timer _timer;
        private readonly IHost _host;
        public MainService(IHost host)
        {
            _host = host;
            _timer = new Timer(60 * 1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
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
