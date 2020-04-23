using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ServerMonitor.Monitors
{
    public sealed class CPUMonitor
    {
        private static readonly CPUMonitor instance = new CPUMonitor();
        private readonly PerformanceCounter pcCpuLoad;
        private CPUMonitor()
        {
            //初始化CPU计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();
            System.Threading.Thread.Sleep(1000);
        }
        public static CPUMonitor GetMonitor()
        {
            return instance;
        }
        public static float GetValue()
        {
            return instance.pcCpuLoad.NextValue();
        }
    }
}
