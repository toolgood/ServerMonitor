using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;
using WebsiteService.MonitorTerminal.Datas.IIS;

namespace WebsiteService.MonitorTerminal.Utils
{
    public static class IISUtil
    {
        #region 获取 所有站点 站点
        public static List<SiteMiniInfo> GetSites()
        {
            List<SiteMiniInfo> siteInfos = new List<SiteMiniInfo>();
            var server = new ServerManager();//请使用管理员模式
            foreach (Site site in server.Sites)
            {
                SiteMiniInfo siteInfo = new SiteMiniInfo()
                {
                    Name = site.Name,
                    Id = site.Id,
                    ServerAutoStart = site.ServerAutoStart,
                    AppPoolName = site.ApplicationDefaults.ApplicationPoolName,
                    SiteState = site.State.ToString(),
                    PhysicalPath = site.Applications["/"].VirtualDirectories["/"].PhysicalPath
                };
                siteInfo.AppPoolName = string.Join(",", site.Applications.Select(q => q.ApplicationPoolName));
                if (site.Applications.Count == 1)
                {
                    var appPool = server.ApplicationPools.Where(q => q.Name == siteInfo.AppPoolName).FirstOrDefault();
                    if (appPool != null)
                    {
                        siteInfo.AppPoolState = appPool.State.ToString();
                    }
                }
                else if (site.Applications.Count > 1)
                {
                    List<string> appPoolStates = new List<string>();
                    foreach (var item in site.Applications)
                    {
                        var appPool = server.ApplicationPools.Where(q => q.Name == item.ApplicationPoolName).FirstOrDefault();
                        if (appPool != null)
                        {
                            if (appPoolStates.Contains(appPool.State.ToString()) == false)
                            {
                                appPoolStates.Add(appPool.State.ToString());
                            }
                        }
                    }
                    if (appPoolStates.Count == 1)
                    {
                        siteInfo.AppPoolState = appPoolStates[0];
                    }
                    else
                    {
                        siteInfo.AppPoolState = "异常";
                    }
                }

                siteInfo.Bindings = new List<string>();
                foreach (Binding item in site.Bindings)
                {
                    string url = item.Protocol + "://";
                    if (item.EndPoint.Address.ToString() == "0.0.0.0")
                    {
                        if (string.IsNullOrEmpty(item.Host))
                        {
                            url += "*:" + item.EndPoint.Port;
                        }
                        else
                        {
                            url += item.Host + ":" + item.EndPoint.Port;
                        }
                    }
                    else
                    {
                        url += item.EndPoint.Address.ToString() + ":" + item.EndPoint.Port;
                    }
                    siteInfo.Bindings.Add(url);
                }
                siteInfos.Add(siteInfo);
            }

            server.Dispose();
            return siteInfos;
        }

        public static SiteInfo GetSiteByName(string siteName)
        {
            using (var server = new ServerManager())
            {
                foreach (var item in server.Sites)
                {
                    if (item.Name != siteName) { continue; }
                    return new SiteInfo(item);
                }
            }
            return null;
        }
        #endregion

        #region 获取应用程序池
        public static List<AppPoolMiniInfo> GetAppPools()
        {
            List<AppPoolMiniInfo> appPools = new List<AppPoolMiniInfo>();
            var server = new ServerManager();//请使用管理员模式
            foreach (var item in server.ApplicationPools)
            {
                appPools.Add(new AppPoolMiniInfo()
                {
                    AutoStart = item.AutoStart,
                    Enable32BitAppOnWin64 = item.Enable32BitAppOnWin64,
                    ManagedPipelineMode = item.ManagedPipelineMode,
                    ManagedRuntimeVersion = item.ManagedRuntimeVersion,
                    StartMode = item.StartMode,
                    Name = item.Name,
                    IdentityType = item.ProcessModel.IdentityType,
                    UserName = item.ProcessModel.UserName
                });
            }
            foreach (Site site in server.Sites)
            {
                foreach (var item in site.Applications)
                {
                    var appPool = appPools.FirstOrDefault(q => q.Name == item.ApplicationPoolName);
                    if (appPool != null)
                    {
                        appPool.SiteCount++;
                    }
                }
            }

            server.Dispose();
            return appPools;
        }

        public static AppPoolInfo GetAppPoolByname(string poolName)
        {
            using (var server = new ServerManager())
            {
                foreach (var item in server.ApplicationPools)
                {
                    if (item.Name != poolName) { continue; }
                    return new AppPoolInfo(item);
                }
            }
            return null;
        }
        #endregion

        #region 站点 新建 
        public static bool CreateSite(string siteName, string port, string physicalPath, string appPoolName, string version = "v4.0", bool isClassic = false)
        {
            using (ServerManager manager = new ServerManager())
            {
                if (manager.Sites.Any(q => q.Name == siteName))
                {
                    return false;
                }

                // 建立站点
                manager.Sites.Add(siteName, "http", string.Format("*:{0}:", port), physicalPath);
                CreateDirectory(physicalPath);
                manager.CommitChanges();

                // 创建App Pool, 配置站点设置
                CreateAppPool(appPoolName, version, isClassic);
                Site site = manager.Sites[siteName];
                Application rootApp = FindRootApplication(site);
                if (rootApp != null)
                {
                    rootApp.ApplicationPoolName = appPoolName;
                }
                manager.CommitChanges();
                return true;
            }
        }

        public static bool AddVirtualDirectory(string siteName, string directoryName, string phyPath, string poolName)
        {
            using (ServerManager manager = new ServerManager())
            {
                Site site = manager.Sites[siteName];
                if (manager.ApplicationPools.Any(q => q.Name == poolName) == false) return false;


                Application app = site.Applications.Add(string.Format("/{0}", directoryName), phyPath);
                CreateDirectory(phyPath);
                app.ApplicationPoolName = poolName;

                manager.CommitChanges();
                return true;
            }
        }

        private static void CreateDirectory(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            try
            {
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                DirectorySecurity security = new DirectorySecurity();
                security.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                dirInfo.SetAccessControl(security);
            }
            catch { }
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
        #endregion

        #region 站点 启动 暂停 重启
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
        public static void DeleteSite(string siteName)
        {
            ServerManager manager = new ServerManager();

            Site site = manager.Sites[siteName];
            manager.Sites.Remove(site);
            manager.CommitChanges();
        }

        #endregion

        #region 应用程序池 新建 
        /// <summary>
        /// 创建应用程序池
        /// </summary>
        /// <param name="appPoolName"></param>
        /// <param name="version"></param>
        /// <param name="isClassic"></param>
        public static bool CreateAppPool(string poolName, string version, bool isClassic)
        {
            using (ServerManager manager = new ServerManager())
            {
                if (manager.ApplicationPools.Any(q => q.Name == poolName) == false)
                {
                    ApplicationPool appPool = manager.ApplicationPools.Add(poolName);
                    appPool.AutoStart = true;
                    appPool.ManagedPipelineMode = isClassic ? ManagedPipelineMode.Classic : ManagedPipelineMode.Integrated;
                    appPool.ManagedRuntimeVersion = version;
                    appPool.ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
                    manager.CommitChanges();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 应用程序池 启动 暂停 删除 回收
        public static void StopAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools.Any(q => q.Name == poolName) == false) { return; }
            if (manager.ApplicationPools[poolName].State != ObjectState.Stopped && manager.ApplicationPools[poolName].State != ObjectState.Stopping)
            {
                manager.ApplicationPools[poolName].Stop();
            }
        }

        public static void StartAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools.Any(q => q.Name == poolName) == false) { return; }
            if (manager.ApplicationPools[poolName].State != ObjectState.Started && manager.ApplicationPools[poolName].State != ObjectState.Starting)
            {
                manager.ApplicationPools[poolName].Start();
            }
        }
        public static void DeleteAppPool(string poolName)
        {
            ServerManager manager = new ServerManager();
            if (manager.ApplicationPools.Any(q => q.Name == poolName) == false) { return; }
            if (manager.ApplicationPools[poolName].State != ObjectState.Stopped && manager.ApplicationPools[poolName].State != ObjectState.Stopping)
            {
                manager.ApplicationPools[poolName].Stop();
            }
            manager.ApplicationPools.Remove(manager.ApplicationPools[poolName]);
            manager.CommitChanges();
        }
        public static string RecycleAppPool(string name)
        {
            ServerManager manager = new ServerManager();
            var pool = manager.ApplicationPools[name];

            return pool.Recycle().ToString();
        }
        #endregion

        #region 获取 可用的证书信息
        public static List<CertificateInfo> GetCertificates()
        {
            List<CertificateInfo> names = new List<CertificateInfo>();
            X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                foreach (var item in certificatesInStore)
                {
                    if (item.NotAfter > DateTime.Now && item.NotBefore < DateTime.Now)
                    {
                        names.Add(new CertificateInfo()
                        {
                            Name = item.FriendlyName,
                            CertificateStoreName = "My",
                            CertificateHashString = item.GetCertHashString()
                        });
                    }
                }
                return names;
            }
            finally
            {
                userCaStore.Close();
            }
        }
        #endregion


    }

}
