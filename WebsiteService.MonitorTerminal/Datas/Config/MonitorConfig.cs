using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas.Config
{
    public class MonitorConfig
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public EmailInfo EmailInfo { get; set; }

        /// <summary>
        /// IIS 网站
        /// </summary>
        public IISWebsiteInfo IISWebsite { get; set; }

        /// <summary>
        /// window 服务
        /// </summary>
        public WindowServiceInfo WindowService { get; set; }

        /// <summary>
        /// 加密信息
        /// </summary>
        public LetsEncryptInfo LetsEncrypt { get; set; }

    }
}
