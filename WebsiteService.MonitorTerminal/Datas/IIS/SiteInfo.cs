using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{

    public class SiteInfo
    {
        public SiteInfo() { }
        public SiteInfo(Site site)
        {
            Id = site.Id;
            Name = site.Name;
            ServerAutoStart = site.ServerAutoStart;
            State = site.State;
            Limits = new SiteLimits2(site.Limits);
            Bindings = new List<Binding2>();
            foreach (var item in site.Bindings)
            {
                Bindings.Add(new Binding2(item));
            }
            Applications = new List<Application2>();
            foreach (var item in site.Applications)
            {
                Applications.Add(new Application2(item));
            }
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public bool ServerAutoStart { get; set; }

        public ObjectState State { get; }

        public SiteLimits2 Limits { get; }

        public List<Binding2> Bindings { get; }

        public List<Application2> Applications { get; }

    }
    public class SiteLimits2
    {
        public SiteLimits2() { }
        public SiteLimits2(SiteLimits siteLimits)
        {
            ConnectionTimeout = siteLimits.ConnectionTimeout;
            MaxBandwidth = siteLimits.MaxBandwidth;
            MaxConnections = siteLimits.MaxConnections;
            MaxUrlSegments = siteLimits.MaxUrlSegments;
        }

        public TimeSpan ConnectionTimeout { get; set; }
        public long MaxBandwidth { get; set; }
        public long MaxConnections { get; set; }
        public long MaxUrlSegments { get; set; }
    }

    public class Binding2
    {
        public Binding2() { }
        public Binding2(Binding binding)
        {
            BindingInformation = binding.BindingInformation;
            CertificateHash = binding.CertificateHash;
            CertificateStoreName = binding.CertificateStoreName;
            EndPoint = binding.EndPoint;
            Host = binding.Host;
            IsIPPortHostBinding = binding.IsIPPortHostBinding;
            SslFlags = binding.SslFlags;
            UseDsMapper = binding.UseDsMapper;
            Protocol = binding.Protocol;
        }
        public string BindingInformation { get; set; }
        public byte[] CertificateHash { get; set; }
        public string CertificateStoreName { get; set; }
        public IPEndPoint EndPoint { get; }
        public string Host { get; }
        public bool IsIPPortHostBinding { get; }
        public SslFlags SslFlags { get; set; }
        public bool UseDsMapper { get; set; }
        public string Protocol { get; set; }

    }
    public class Application2
    {
        public Application2() { }
        public Application2(Application application)
        {
            ApplicationPoolName = application.ApplicationPoolName;
            EnabledProtocols = application.EnabledProtocols;
            Path = application.Path;
            VirtualDirectories = new List<VirtualDirectory2>();
            foreach (var item in application.VirtualDirectories)
            {
                VirtualDirectories.Add(new VirtualDirectory2(item));
            }
        }

        public string ApplicationPoolName { get; set; }
        public string EnabledProtocols { get; set; }
        public string Path { get; set; }
        public List<VirtualDirectory2> VirtualDirectories { get; set; }
    }
    public class VirtualDirectory2
    {
        public VirtualDirectory2() { }
        public VirtualDirectory2(VirtualDirectory virtualDirectory)
        {
            LogonMethod = virtualDirectory.LogonMethod;
            Password = virtualDirectory.Password;
            Path = virtualDirectory.Path;
            PhysicalPath = virtualDirectory.PhysicalPath;
            UserName = virtualDirectory.UserName;
        }

        public AuthenticationLogonMethod LogonMethod { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
        public string PhysicalPath { get; set; }
        public string UserName { get; set; }
    }
 

}
