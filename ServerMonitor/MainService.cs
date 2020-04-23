using ServerMonitor.Utils;
using System.Timers;

namespace ServerMonitor
{
    public class MainService
    {
        readonly Timer _timer;
        public MainService()
        {
            _timer = new Timer(60 * 1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var monitorInfo = MonitorUtil.GetMachineMonitorInfo();
            var sites = MonitorUtil.GetSiteInfos();


        }

        public void Start() { _timer.Start(); Timer_Elapsed(_timer, null); }
        public void Stop() { _timer.Stop(); }
    }
}
