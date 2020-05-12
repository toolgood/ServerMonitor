using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class SiteInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PhysicalPath { get; set; }

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
