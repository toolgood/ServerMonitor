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
        readonly Timer _timer;
        private IHost _host;
        public MainService(IHost host)
        {
            _host = host;
            _timer = new Timer(60 * 1000) { AutoReset = true };
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //var monitorInfo = MonitorUtil.GetMachineMonitorInfo();
            //var sites = MonitorUtil.GetSiteInfos();
        }
        public void Start()
        {
            _timer_Elapsed(_timer, null);
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
