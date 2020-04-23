using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Monitors
{
    public sealed class CPUMonitor
    {
        private static readonly CPUMonitor instance = new CPUMonitor();
        private PerformanceCounter pcCpuLoad;
        private CPUMonitor()
        {
            //初始化CPU计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();
            System.Threading.Thread.Sleep(1000);
        }
        public static CPUMonitor getMonitor()
        {
            return instance;
        }
        public static float getValue()
        {
            return instance.pcCpuLoad.NextValue();
        }
    }
}
