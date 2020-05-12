using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class SoftwareInfo
    {
        /// <summary>
        /// 软件名
        /// </summary>
        public string Name { get; set; }//DisplayName
        /// <summary>
        /// 软件图标
        /// </summary>
        public string Icon { get; set; }//DisplayIcon
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }//DisplayVersion
        /// <summary>
        /// 安装地址
        /// </summary>
        public string InstallLocation { get; set; } //InstallLocation  InstallDir 
        /// <summary>
        /// 公司名
        /// </summary>
        public string Publisher { get; set; }//Publisher   Microsoft Corporation
        /// <summary>
        /// 安装日期
        /// </summary>
        public string InstallDate { get; set; }//InstallDate 20191122

    }
}
