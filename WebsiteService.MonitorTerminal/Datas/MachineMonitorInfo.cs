using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class MachineMonitorInfo
    {
        /// <summary>
        /// CPU 使用率
        /// </summary>
        public float CpuUsage { get; set; }
        /// <summary>
        /// 核心数
        /// </summary>
        public int CoreNumber { get; set; }
        /// <summary>
        /// 可用内存 GB
        /// </summary>
        public float MemoryAvailable { get; set; }
        /// <summary>
        /// 物理内存 GB
        /// </summary>
        public float PhysicalMemory { get; set; }

        /// <summary>
        /// 硬盘信息
        /// </summary>
        public List<HardDiskInfo> HardDisks { get; set; }

        /// <summary>
        /// 磁盘读速度 KB
        /// </summary>
        public int DiskReadData { get; set; }
        /// <summary>
        /// 磁盘写速度 KB
        /// </summary>
        public int DiskWriteData { get; set; }
        /// <summary>
        /// 网络下载 KB
        /// </summary>
        public int NetworkReceiveData { get; set; }
        /// <summary>
        /// 网络上传 KB
        /// </summary>
        public int NetworkSendData { get; set; }

        public override string ToString()
        {
            return $"CPU:{CoreNumber}核，使用率{CpuUsage:0.00}%，可用内存{MemoryAvailable:0.00}GB，总共内存{PhysicalMemory:0.00}GB";
        }

    }

}
