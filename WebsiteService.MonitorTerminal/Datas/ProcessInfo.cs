using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 应用程序名
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 监控名
        /// </summary>
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
