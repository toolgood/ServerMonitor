using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class CertificateInfo
    {
        public string Name { get; set; }
        public string CertificateHashString { get; set; }
        public string CertificateStoreName { get; set; }
    }
}
