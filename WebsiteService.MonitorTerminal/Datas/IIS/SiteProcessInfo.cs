using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class SiteProcessInfo
    {
        public string Name { get; set; }

        public Dictionary<string, ProcessInfo> ProcessInfo { get; set; }

    }
}
