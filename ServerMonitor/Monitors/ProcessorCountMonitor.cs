using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerMonitor.Monitors
{
    public sealed class ProcessorCountMonitor
    {
        public static readonly ProcessorCountMonitor instance = new ProcessorCountMonitor();
        private int m_ProcessorCount = 0;   //CPU个数
        private ProcessorCountMonitor()
        {
            //CPU个数
            m_ProcessorCount = Environment.ProcessorCount;
        }
        public static ProcessorCountMonitor getMonitor()
        {
            return instance;
        }
        public static int getValue()
        {
            return getMonitor().m_ProcessorCount;
        }
    }
}
