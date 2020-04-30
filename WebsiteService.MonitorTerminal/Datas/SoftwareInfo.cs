using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class SoftwareInfo
    {
        public string Name { get; set; }//DisplayName
        public string Icon { get; set; }//DisplayIcon
        public string Version { get; set; }//DisplayVersion
        public string InstallLocation { get; set; } //InstallLocation  InstallDir 
        public string Publisher { get; set; }//Publisher   Microsoft Corporation
        public string InstallDate { get; set; }//InstallDate 20191122

    }
}
