using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Datas
{
    public class HardDiskInfo
    {
        public string Name { get; set; }
        /// <summary>
        /// 硬盘可用空间 GB
        /// </summary>
        public float FreeSpace { get; set; }

        /// <summary>
        /// 硬盘空间 GB
        /// </summary>
        public float Space { get; set; }

        public override string ToString()
        {
            return $"{Name}盘,可用{FreeSpace:0.00}GB,总共{Space:0.00}GB";
        }
    }

}
