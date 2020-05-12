using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class FolderInfo
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Extension { get; set; }
        /// <summary>
        /// 文件大小 B
        /// </summary>
        [JsonProperty(NullValueHandling= NullValueHandling.Ignore)]
        public long? Size { get; set; }

        /// <summary>
        /// 类型，0 文件，1 文件夹，2 硬盘
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastWriteTime  { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreationTime  { get; set; }


    }
}
