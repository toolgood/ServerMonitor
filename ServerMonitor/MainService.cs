using ServerMonitor.Monitors;
using ServerMonitor.Utils;
using System.Linq;
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
            ProcessMonitor.Init();
            ProcessMonitor.UpdateProcessList();
            var pl = ProcessMonitor.ProcessList;
            var pl2 = pl.ToList().OrderByDescending(q => q.CpuUsage).ToList();
        }

        public void Start() { _timer.Start(); Timer_Elapsed(_timer, null); }
        public void Stop() { _timer.Stop(); }
    }
}
