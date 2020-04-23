using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Monitors
{
    public sealed class DiskWriteMonitor
    {
        private static readonly DiskWriteMonitor instance = new DiskWriteMonitor();
        private readonly PerformanceCounter counter;

        private DiskWriteMonitor()
        {
            counter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            counter.NextValue();
            System.Threading.Thread.Sleep(1000);
        }

        public static DiskWriteMonitor GetMonitor()
        {
            return instance;
        }

        public static float GetValue()
        {
            return instance.counter.NextValue();
        }
    }
}
