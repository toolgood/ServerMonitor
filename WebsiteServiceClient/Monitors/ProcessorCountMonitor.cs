using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Monitors
{
    public sealed class ProcessorCountMonitor
    {
        public static readonly ProcessorCountMonitor instance = new ProcessorCountMonitor();
        private readonly int m_ProcessorCount = 0;   //CPU个数
        private ProcessorCountMonitor()
        {
            //CPU个数
            m_ProcessorCount = Environment.ProcessorCount;
        }
        public static ProcessorCountMonitor GetMonitor()
        {
            return instance;
        }
        public static int GetValue()
        {
            return GetMonitor().m_ProcessorCount;
        }
    }
}
