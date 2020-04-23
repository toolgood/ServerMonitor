using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Utils
{
    /// <summary>
    ///  操作类
    /// </summary>
    public class ServerUtil
    {
        /// <summary>
        /// 获取所有服务
        /// </summary>
        public static List<ServiceController> GetAllServices()
        {
            var list = System.ServiceProcess.ServiceController.GetServices().ToList();
            list.RemoveAll(q => q.MachineName != ".");
            list.RemoveAll(q => q.ServiceType != ServiceType.Win32OwnProcess);
            return list;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="strServiceName">服务名</param>
        /// <returns></returns>
        public static ServiceController GetServiceByName(string strServiceName)
        {
            try {
                foreach (ServiceController sc in GetAllServices()) {
                    if (sc.ServiceName.ToLower().Trim() == strServiceName.ToLower().Trim()) { return sc; }
                }
            } catch { }
            return null;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="service">服务对象</param>
        /// <returns></returns>
        public static bool StopService(System.ServiceProcess.ServiceController service)
        {
            try {
                if (service == null) return false;
                if (service.CanStop && service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending) {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    return true;
                }
            } catch { }
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
            try {
                if (service == null) return false;
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending) {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
            } catch { }
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
            try {
                if (service == null) return false;
                if (StopService(service))
                    return StartService(service);
            } catch { }
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
            try {
                if (service == null) return false;
                if (service.Status != ServiceControllerStatus.Paused && service.Status != ServiceControllerStatus.PausePending) {
                    service.Pause();
                    service.WaitForStatus(ServiceControllerStatus.Paused);
                    return true;
                }
            } catch { }
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
        public bool ResumeService(System.ServiceProcess.ServiceController service)
        {
            try {
                if (service == null) return false;
                if (service.Status == ServiceControllerStatus.Paused) {
                    service.Continue();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
            } catch { }
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
