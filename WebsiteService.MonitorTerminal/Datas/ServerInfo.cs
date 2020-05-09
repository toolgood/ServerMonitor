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
        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ServiceType ServiceType { get; }

        public ServiceStartMode StartType { get; }

        public ServiceControllerStatus Status { get; }

        public string SrcFilePath { get; set; }

        public string FilePath { get; set; }
    }

}
