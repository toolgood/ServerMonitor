using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{

    public class SiteInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool ServerAutoStart { get; set; }

        public ObjectState State { get; }

        public SiteLimits Limits { get; }

        public BindingCollection Bindings { get; }

        public ApplicationCollection Applications { get; }

    }

}
