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
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var monitorInfo = MonitorUtil.GetMachineMonitorInfo();



        }

        public void Start() { _timer.Start(); _timer_Elapsed(_timer, null); }
        public void Stop() { _timer.Stop(); }
    }
}
