using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Monitors
{
    public sealed class NetworkSendMonitor
    {
        private static readonly NetworkSendMonitor instance = new NetworkSendMonitor();
        private readonly List<PerformanceCounter> counters = new List<PerformanceCounter>();

        private NetworkSendMonitor()
        {
            //初始化CPU计数器
            //counter = new PerformanceCounter("Network Interface", "Bytes Received/sec", "*");

            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string[] instances = performanceCounterCategory.GetInstanceNames(); // 多网卡的要确定当前用的是哪个
            //发送
            foreach (string instance in instances) {
                PerformanceCounter counter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
                counters.Add(counter);
                counter.NextValue();
            }

            System.Threading.Thread.Sleep(1000);
        }

        public static NetworkSendMonitor GetMonitor()
        {
            return instance;
        }

        public static float GetValue()
        {
            float value = 0;
            //return instance.counter.NextValue();
            foreach (PerformanceCounter counter in instance.counters) {
                value += counter.NextValue();
            }
            return value;
        }
    }

}
