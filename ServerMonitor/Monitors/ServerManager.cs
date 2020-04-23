using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace ServerMonitor.Monitors
{
    /// <summary>
    /// 服务e799bee5baa6e4b893e5b19e31333332613764操作类
    /// </summary>
    public class ServerManager  
    {
        /// <summary>
        /// 获取所有服务
        /// </summary>
        public ServiceController[] getAllServices {
            get {
                return System.ServiceProcess.ServiceController.GetServices();
            }
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="strServiceName">服务名</param>
        /// <returns></returns>
        public ServiceController getServiceByName(string strServiceName)
        {
            try
            {
                foreach (ServiceController sc in ServiceController.GetServices())
                {
                    if (sc.ServiceName.ToLower().Trim() == strServiceName.ToLower().Trim())
                    {
                        return sc;
                    }
                } //end foreach
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取指定类型的服务
        /// </summary>
        /// <param name="ServiceType">服务类型</param>
        /// <returns></returns>
        public object GetService(System.Type ServiceType)
        {
            ServiceController[] allServices = getAllServices;
            foreach (ServiceController sc in allServices)
            {
                if (sc.ServiceType is ServiceType)
                {
                    //sc.
                    return sc;
                }
            }
            return null;
        } /// <summary>
          /// 停止服务
          /// </summary>
          /// <param name="Service">服务对象</param>
          /// <returns></returns>
        public bool stopService(System.ServiceProcess.ServiceController Service)
        {
            try
            {
                if (Service.CanStop && Service.Status != ServiceControllerStatus.Stopped && Service.Status != ServiceControllerStatus.StopPending)
                {
                    Service.Stop();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        } /// <summary>
          /// 启动服务
          /// </summary>
          /// <param name="Service">服务对象</param>
          /// <returns></returns>
        public bool startService(System.ServiceProcess.ServiceController Service)
        {
            try
            {
                if (Service.Status != ServiceControllerStatus.Running && Service.Status != ServiceControllerStatus.StartPending)
                {
                    Service.Start();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        } /// <summary>
          /// 重启服务
          /// </summary>
          /// <param name="Service"></param>
          /// <returns></returns>
        public bool resetService(System.ServiceProcess.ServiceController Service)
        {
            try
            {
                if (stopService(Service))
                    return startService(Service);
                return false;
            }
            catch
            { return false; }
        } /// <summary>
          /// 暂停服务
          /// </summary>
          /// <param name="Service"></param>
          /// <returns></returns>
        public bool preService(System.ServiceProcess.ServiceController Service)
        {
            try
            {
                if (Service.Status != ServiceControllerStatus.Paused && Service.Status != ServiceControllerStatus.PausePending)
                {
                    Service.Pause();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        } /// <summary>
          /// 继续服务
          /// </summary>
          /// <param name="Service"></param>
          /// <returns></returns>
        public bool resumeService(System.ServiceProcess.ServiceController Service)
        {
            try
            {
                if (Service.Status == ServiceControllerStatus.Paused)
                {
                    Service.Continue();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

}
