using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class FolderInfo
    {
        public string Path { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string Extension { get; set; }

        [JsonIgnore]
        public long? Size { get; set; }

        /// <summary>
        /// 类型，0 文件，1 文件夹，2 硬盘
        /// </summary>
        public int FileType { get; set; }

        [JsonIgnore]
        public DateTime? LastWriteTime  { get; set; }

        [JsonIgnore]
        public DateTime? CreationTime  { get; set; }


    }
}
