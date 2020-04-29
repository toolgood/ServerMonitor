using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Monitors
{
    public sealed class DiskReadMonitor
    {
        private static readonly DiskReadMonitor instance = new DiskReadMonitor();
        private readonly PerformanceCounter counter;

        private DiskReadMonitor()
        {
            counter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            counter.NextValue();
            System.Threading.Thread.Sleep(1000);
        }

        public static DiskReadMonitor GetMonitor()
        {
            return instance;
        }

        public static float GetValue()
        {
            return instance.counter.NextValue();
        }
    }
}
