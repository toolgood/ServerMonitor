using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class ServerInfo
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public ServiceType ServiceType { get; }
        /// <summary>
        /// 打开类型
        /// </summary>
        public ServiceStartMode StartType { get; }
        /// <summary>
        /// 状态
        /// </summary>
        public ServiceControllerStatus Status { get; }

        /// <summary>
        /// 注册表中的文件地址
        /// </summary>
        public string SrcFilePath { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; set; }
    }

}
