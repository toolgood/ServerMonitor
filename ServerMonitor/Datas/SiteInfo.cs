using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Datas
{
    public class SiteInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhysicalPath { get; set; }
        public bool ServerAutoStart { get; set; }
        public string AppPoolName { get; set; }
        public SiteState SiteState { get; set; }
        public SiteState AppPoolState { get; set; }
        public List<SiteBinding> Bindings { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name}[{SiteState}],{AppPoolName}[{AppPoolState}],{(ServerAutoStart ? "自动启动" : "关闭")}";
        }
    }
    public enum SiteState
    {
        Starting = 0,
        Started = 1,
        Stopping = 2,
        Stopped = 3,
        Unknown = 4
    }
    public class SiteBinding
    {
       
        public string BindingInformation { get; set; }
        //public byte[] CertificateHash { get; set; }
        //public string CertificateStoreName { get; set; }
        //public IPEndPoint EndPoint { get; }
        public string Host { get; set; }
        public bool IsIPPortHostBinding { get; set; }
        //public bool UseDsMapper { get; set; }
        public string Protocol { get; set; }

        public override string ToString()
        {
            return $"{Protocol},{Host},{BindingInformation}";
        }
    }
}
