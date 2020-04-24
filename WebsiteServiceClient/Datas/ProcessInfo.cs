using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Datas
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string ProcessName { get; set; }
        public string FileName { get; set; }
        public string InstanceName { get; set; }
        /// <summary>
        /// 内存使用量 MB
        /// </summary>
        public float MemoryUsage { get; set; }
        /// <summary>
        /// CUP使用量 %
        /// </summary>
        public float CpuUsage { get; set; }

    }
}
