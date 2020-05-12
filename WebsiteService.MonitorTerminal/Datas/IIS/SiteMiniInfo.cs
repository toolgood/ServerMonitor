using System.Collections.Generic;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class SiteMiniInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 物理地址
        /// </summary>
        public string PhysicalPath { get; set; }
        /// <summary>
        /// 自动启动
        /// </summary>
        public bool ServerAutoStart { get; set; }

        public string AppPoolName { get; set; }

        public string SiteState { get; set; }

        public string AppPoolState { get; set; }

        public List<string> Bindings { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name}[{SiteState}],{AppPoolName}[{AppPoolState}],{(ServerAutoStart ? "自动启动" : "关闭")}";
        }
    }

}
