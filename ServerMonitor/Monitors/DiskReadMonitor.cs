using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Monitors
{
    public sealed class DiskReadMonitor
    {
        private static readonly DiskReadMonitor instance = new DiskReadMonitor();
        private PerformanceCounter counter;

        private DiskReadMonitor()
        {
            counter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            counter.NextValue();
            System.Threading.Thread.Sleep(1000);
        }

        public static DiskReadMonitor getMonitor()
        {
            return instance;
        }

        public static float getValue()
        {
            return instance.counter.NextValue();
        }
    }
 
}
