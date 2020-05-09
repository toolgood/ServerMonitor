using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using WebsiteService.MonitorTerminal.Datas;

namespace WebsiteService.MonitorTerminal.Utils
{
    /// <summary>
    ///  操作类
    /// </summary>
    public static class ServerUtil
    {

        [DllImport("kernel32.dll")]
        private extern static IntPtr GetModuleHandle(string lpLibFileNmae);

        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);

        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr hModule);

        [DllImport("user32.dll")]
        private extern static int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);


        /// <summary>
        /// 获取所有服务 排除 c:\Windows\ 系统目录下，防止出现一些无法解决的bug
        /// </summary>
        public static List<ServerInfo> GetAllServices()
        {
            var list = System.ServiceProcess.ServiceController.GetServices().ToList();
            list.RemoveAll(q => q.MachineName != ".");
            list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.FileSystemDriver));
            list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.KernelDriver));
            list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.RecognizerDriver));

            list.RemoveAll(q => q.StartType == ServiceStartMode.Boot);
            list.RemoveAll(q => q.StartType == ServiceStartMode.System);

            List<ServerInfo> servers = new List<ServerInfo>();
            RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services", false);
            foreach (var item in list)
            {
                var server = new ServerInfo()
                {
                    ServiceName = item.ServiceName,
                    DisplayName = item.DisplayName,
                };
                var key = servicesKey.OpenSubKey(item.ServiceName);
                var path = key.GetValue("ImagePath") as string;
                if (path == null) { key.Close(); continue; }

                server.SrcFilePath = path;
                server.FilePath = Environment.ExpandEnvironmentVariables(path);
                server.Description = key.GetValue("Description") as string;
                if (server.FilePath.StartsWith("\""))
                {
                    server.FilePath = server.FilePath.Trim('"');
                }
                if (server.FilePath.ToLower().StartsWith("c:\\windows\\")) { key.Close(); continue; }
                servers.Add(server);

                if (server.Description != null)
                {
                    server.Description = server.Description.Trim();
                    server.Description = server.Description.Trim((char) 0);
                    if (server.Description.StartsWith("@"))
                    {
                        var str = server.Description.Substring(1).Split(',');
                        var filePath = Environment.ExpandEnvironmentVariables(str[0]);
                        if (File.Exists(filePath))
                        {
                            IntPtr h = IntPtr.Zero;
                            uint uid = 0;
                            if (str.Length > 1 && filePath.StartsWith("c:\\Windows\\", StringComparison.CurrentCultureIgnoreCase) == false)
                            {
                                if (uint.TryParse(str[1].Replace("-", ""), out uid))
                                {
                                    StringBuilder stringBuilder = new StringBuilder(2048);
                                    h = GetModuleHandle(filePath);
                                    if (h != IntPtr.Zero)
                                    {
                                        LoadString(h, uid, stringBuilder, 2048);
                                    }
                                    else
                                    {
                                        h = LoadLibrary(filePath);
                                        LoadString(h, uid, stringBuilder, 2048);
                                    }
                                    FreeLibrary(h);
                                    server.Description = stringBuilder.ToString();
                                }
                            }
                        }
                    }
                }
                key.Close();
            }
            servicesKey.Close();
            return servers;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="strServiceName">服务名</param>
        /// <returns></returns>
        public static ServiceController GetServiceByName(string strServiceName)
        {
            try
            {
                var list = System.ServiceProcess.ServiceController.GetServices().ToList();
                list.RemoveAll(q => q.MachineName != ".");
                list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.FileSystemDriver));
                list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.KernelDriver));
                list.RemoveAll(q => q.ServiceType.HasFlag(ServiceType.RecognizerDriver));

                list.RemoveAll(q => q.StartType == ServiceStartMode.Boot);
                list.RemoveAll(q => q.StartType == ServiceStartMode.System);

                foreach (ServiceController sc in list)
                {
                    if (sc.ServiceName.ToLower().Trim() == strServiceName.ToLower().Trim())
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\" + sc.ServiceName, false);
                        var path = key.GetValue("ImagePath") as string;
                        if (path != null)
                        {
                            path = Environment.ExpandEnvironmentVariables(path).Trim('"');
                            if (path.ToLower().StartsWith("c:\\windows\\") == false)
                            {
                                key.Close();
                                return sc;
                            }
                        }
                        key.Close();
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="service">服务对象</param>
        /// <returns></returns>
        public static bool StopService(System.ServiceProcess.ServiceController service)
        {
            try
            {
                if (service == null) return false;
                if (service.CanStop && service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    return true;
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool StopService(string serviceName)
        {
            var service = GetServiceByName(serviceName);
            return StopService(service);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="service">服务对象</param>
        /// <returns></returns>
        public static bool StartService(System.ServiceProcess.ServiceController service)
        {
            try
            {
                if (service == null) return false;
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool StartService(string serviceName)
        {
            var service = GetServiceByName(serviceName);
            return StartService(service);
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool ResetService(System.ServiceProcess.ServiceController service)
        {
            try
            {
                if (service == null) return false;
                if (StopService(service))
                    return StartService(service);
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool ResetService(string serviceName)
        {
            var service = GetServiceByName(serviceName);
            return ResetService(service);
        }



        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool PauseService(System.ServiceProcess.ServiceController service)
        {
            try
            {
                if (service == null) return false;
                if (service.Status != ServiceControllerStatus.Paused && service.Status != ServiceControllerStatus.PausePending)
                {
                    service.Pause();
                    service.WaitForStatus(ServiceControllerStatus.Paused);
                    return true;
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool PauseService(string serviceName)
        {
            var service = GetServiceByName(serviceName);
            return PauseService(service);
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool ResumeService(System.ServiceProcess.ServiceController service)
        {
            try
            {
                if (service == null) return false;
                if (service.Status == ServiceControllerStatus.Paused)
                {
                    service.Continue();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 继续服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool ResumeService(string serviceName)
        {
            var service = GetServiceByName(serviceName);
            return PauseService(service);
        }


    }
}
