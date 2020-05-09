﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using WebsiteService.MonitorTerminal.Datas;

namespace WebsiteService.MonitorTerminal.Utils
{
    public static class IISUtil
    {
        public static void ResetSite(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];
            site.Stop();
            site.Start();
        }

        public static string StopSite(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];
            return site.Stop().ToString();
        }

        public static string StartSite(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];
            var s = site.Start().ToString();
            StartAppPool(site.ApplicationDefaults.ApplicationPoolName);
            return s;
        }

        public static string GetSiteState(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];

            return site.State.ToString();
        }

        public static bool DoesExistSite(string siteName)
        {
            try
            {
                ServerManager manager = new ServerManager();

                Site site = manager.Sites[siteName];
                return site != null;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteSite(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];
            manager.Sites.Remove(site);
            manager.CommitChanges();
        }

        private static void CreateDirectory(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            CreateIISDirectory(dirInfo);
        }

        private static bool CreateIISDirectory(DirectoryInfo dirInfo)
        {
            try
            {
                DirectorySecurity security = new DirectorySecurity();
                security.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                // 设置Everyone权限
                dirInfo.SetAccessControl(security);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <param name="version"></param>
        /// <param name="isClassic"></param>
        public static void CreateAppPool(string appPoolName, string version, bool isClassic)
        {
            if (!DoesExistAppPool(appPoolName))
            {
                ServerManager iisManager = new ServerManager();
                ApplicationPool appPool = iisManager.ApplicationPools.Add(appPoolName);
                appPool.AutoStart = true;
                appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                appPool.ManagedRuntimeVersion = "v2.0";
                iisManager.CommitChanges();
            }

            ServerManager manager = new ServerManager();

            manager.ApplicationPools[appPoolName].ManagedRuntimeVersion = version;
            manager.ApplicationPools[appPoolName].ManagedPipelineMode = isClassic ?
                ManagedPipelineMode.Classic : ManagedPipelineMode.Integrated;
            manager.ApplicationPools[appPoolName].ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
            manager.CommitChanges();
        }

        /// <summary>
        /// 应用程序池是否存在
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <returns></returns>
        public static bool DoesExistAppPool(string appPoolName)
        {
            ServerManager manager = new ServerManager();

            ApplicationPool pool;

            pool = null;
            try
            {
                pool = manager.ApplicationPools[appPoolName];
            }
            catch
            {
            }
            manager.Dispose();
            return pool != null;
        }


        private static Application FindRootApplication(Site site)
        {
            foreach (Application app in site.Applications)
            {
                if (!string.IsNullOrEmpty(app.Path) && app.Path == "/")
                {
                    return app;
                }
            }
            return null;
        }

        public static Site CreateSite(string siteName, string port, string appPoolName, string baseFolder)
        {
            ServerManager manager = new ServerManager();

            // 如站点存在，删除
            if (DoesExistSite(siteName))
            {
                DeleteSite(siteName);
            }

            // 建立站点
            string physicalPath = Path.Combine(baseFolder, siteName);
            manager.Sites.Add(siteName, "http", string.Format("*:{0}:", port), physicalPath);
            CreateDirectory(physicalPath);
            manager.CommitChanges();

            // 创建App Pool, 配置站点设置
            CreateAppPool(appPoolName, "v4.0", false);
            Site site = manager.Sites[siteName];
            Application rootApp = FindRootApplication(site);
            if (rootApp != null)
            {
                rootApp.ApplicationPoolName = appPoolName;
                manager.CommitChanges();
            }
            site = manager.Sites[siteName];
            manager.CommitChanges();

            return site;
        }

        public static void CreateApplication(string siteName, string appName, string appPoolName, string baseFolder)
        {
            if (!DoesExistAppPool(appPoolName))
            {
                CreateAppPool(appPoolName, "v4.0", false);
            }

            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];

            string phyPath = Path.Combine(baseFolder, appName);

            Application app = site.Applications.Add(string.Format("/{0}", appName), phyPath);
            CreateDirectory(phyPath);
            app.ApplicationPoolName = appPoolName;

            manager.CommitChanges();
        }

        public static void StopAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools[poolName].State != ObjectState.Stopped && manager.ApplicationPools[poolName].State != ObjectState.Stopping)
            {
                manager.ApplicationPools[poolName].Stop();
            }
        }

        public static void StartAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools[poolName].State != ObjectState.Started && manager.ApplicationPools[poolName].State != ObjectState.Starting)
            {
                manager.ApplicationPools[poolName].Start();
            }
        }
        public static void DeleteAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools[poolName].State != ObjectState.Stopped && manager.ApplicationPools[poolName].State != ObjectState.Stopping)
            {
                manager.ApplicationPools[poolName].Stop();
            }
            manager.ApplicationPools.Remove(manager.ApplicationPools[poolName]);
        }
        //public static void StopSitePools(string siteName)
        //{
        //    ServerManager manager = new ServerManager();
        //    IEnumerable<Site> sites = manager.Sites.Where(s => s.Name.Equals(siteName));
        //    if (sites != null && sites.Count() > 0)
        //    {
        //        Site site = sites.First();
        //        List<string> appPools = new List<string>();

        //        var apps = site.Applications;
        //        foreach (var app in apps)
        //        {
        //            //app.ApplicationPoolName
        //        }
        //    }
        //}

        public static void KeepPoolActive(string poolName)
        {
            ServerManager manager = new ServerManager();
            var pool = manager.ApplicationPools[poolName];

            pool.Recycling.PeriodicRestart.Time = new TimeSpan(0, 43200, 0);
            pool.ProcessModel.IdleTimeout = new TimeSpan(0, 43200, 0);
            pool.ProcessModel.MaxProcesses = 5;

            manager.CommitChanges();
        }

        public static List<string> GetAllAppPoolNames()
        {
            ServerManager manager = new ServerManager();
            return manager.ApplicationPools.Select<ApplicationPool, string>(o => o.Name).ToList();
        }

        public static string RecycleAppPool(string name)
        {
            ServerManager manager = new ServerManager();
            var pool = manager.ApplicationPools[name];

            return pool.Recycle().ToString();
        }

        public static ObjectState GetAppPoolSatus(string poolName)
        {
            ServerManager manager = new ServerManager();
            return manager.ApplicationPools[poolName].State;
        }

        public static ApplicationPool GetAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            return manager.ApplicationPools[poolName];
        }


        public static List<SiteInfo> GetSites()
        {
            List<SiteInfo> siteInfos = new List<SiteInfo>();
            var server = new ServerManager();//请使用管理员模式
            foreach (Site site in server.Sites)
            {
                SiteInfo siteInfo = new SiteInfo()
                {
                    Name = site.Name,
                    Id = site.Id,
                    ServerAutoStart = site.ServerAutoStart,
                    AppPoolName = site.ApplicationDefaults.ApplicationPoolName,
                    SiteState = site.State.ToString(),
                    PhysicalPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath

                };
                // s.Pool = site.Applications["/"].ApplicationPoolName;
                var appPool = server.ApplicationPools.Where(q => q.Name == siteInfo.AppPoolName).FirstOrDefault();
                if (appPool != null)
                {
                    siteInfo.AppPoolState = appPool.State.ToString();
                }
                siteInfo.Bindings = new List<string>();
                foreach (Binding item in site.Bindings)
                {
                    string url = item.Protocol + "://";
                    if (item.EndPoint.Address.ToString() == "0.0.0.0")
                    {
                        url += item.Host + ":" + item.EndPoint.Port;
                    }
                    else
                    {
                        url += item.EndPoint.Address.ToString() + ":" + item.EndPoint.Port;
                    }
                    siteInfo.Bindings.Add(url);

                    //SiteBinding siteBinding = new SiteBinding() {
                    //    BindingInformation = item.BindingInformation,
                    //    Protocol = item.Protocol,
                    //    Host = item.Host,
                    //    IsIPPortHostBinding = item.IsIPPortHostBinding
                    //};
                    //siteInfo.Bindings.Add(siteBinding);
                }
                siteInfos.Add(siteInfo);
            }

            return siteInfos;
        }

    }

}
