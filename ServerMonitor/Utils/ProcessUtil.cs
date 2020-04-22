using ServerMonitor.Monitors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Utils
{
    public class ProcessUtil
    {
        public static string GetInstanceName(string categoryName, string counterName, Process p)
        {
            try {
                PerformanceCounterCategory processcounter = new PerformanceCounterCategory(categoryName);
                string[] instances = processcounter.GetInstanceNames();
                foreach (string instance in instances) {
                    PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instance);
                    //Logger.Info("对比in mothod GetInstanceName，" + counter.NextValue() + "：" + p.Id);
                    if (counter.NextValue() == p.Id) {
                        return instance;
                    }
                }
            } catch (Exception ex) {
            }
            return null;
        }
        public static PerformanceCounter GetProcessorTime(Process p)
        {
            string instance1 = GetInstanceName("Process", "ID Process", p);
            if (instance1 != null) {
                PerformanceCounter cpucounter = new PerformanceCounter("Process", "% Processor Time", instance1);
                if (cpucounter != null) {
                    cpucounter.NextValue();
                    System.Threading.Thread.Sleep(200); //等200ms(是测出能换取下个样本的最小时间间隔)，让后系统获取下一个样本,因为第一个样本无效
                    return cpucounter;
                } else {
                    //Logger.Info("Name:" + name + "生成CPU监控失败" + instance1);
                }
            } else {
                //Logger.Info("Name:" + name + "获取cpu监控实例失败" + instance1);
            }
            return null;
        }

        public static PerformanceCounter GetGC(Process p)
        {
            // 获取GC占用率 PerformanceCounter            
            string instance2 = GetInstanceName(".NET CLR Memory", "Process ID", p);
            if (instance2 != null) {
                PerformanceCounter gccounter = new PerformanceCounter(".NET CLR Memory", "% Time in GC", instance2);
                if (gccounter != null) {
                    return gccounter;
                    //Logger.Info("Name:" + name + "生成GC监控成功" + instance2);
                } else {
                    //Logger.Info("Name:" + name + "生成GC监控失败" + instance2);
                }
            } else {
                //Logger.Info("Name:" + name + "获取GC监控实例失败" + instance2);
            }
            return null;
        }


        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static bool IISReset()
        {
            //Logger.Info("Start IISRESET");
            int flag = 0;
            string output = IISResetProcess();
            //Logger.Info(output);
            string regx = "正在尝试停止...\r\r\nInternet 服务已成功停止\r\r\n正在尝试启动...\r\r\nInternet 服务已成功启动";
            while (flag < 10 && !output.Contains(regx)) {
                output = IISResetProcess();
                //Logger.Info(output);
                flag++;
            }
            bool result = output.Contains(regx);
            //Logger.Info("Finish IISRESET - result : " + result);
            return result;
        }

        public static string IISResetProcess()
        {
            Process pro = new Process();
            // 设置命令行、参数
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;
            // 启动CMD
            pro.Start();
            // 运行端口检查命令
            pro.StandardInput.WriteLine("iisreset");
            pro.StandardInput.WriteLine("exit");
            return pro.StandardOutput.ReadToEnd();
        }


        //public string StartMonitorMachine(MachineMonitorConfigPack info)
        //{
        //    var resp = new APIResponse<string>() {
        //        StateCode = StateCode.Success,
        //        Message = "启动监控成功"
        //    };

        //    try {
        //        // String name = ConfigurationManager.AppSettings["name"];
        //        // String pwd = ConfigurationManager.AppSettings["password"];
        //        // String domain = ".";

        //        //Logger.Info("domain:" + domain + "name:" + name + ",password:" + pwd);

        //        ImpersonateUser user = new ImpersonateUser();
        //        ImpersonateUserExt.ImpersonateAdminUser(user);

        //        //DateTime startTime = DateTime.Parse(info.startTime);
        //        DateTime startTime = DateTime.Now;
        //        DateTime endTime = startTime.AddMinutes(info.lastTime);

        //        MachineMonitor.SetStartTime(startTime);
        //        MachineMonitor.SetDeadTime(endTime);
        //        MachineMonitor.SetRecalUrl(info.ReCallUrl);
        //        MachineMonitor.SetConfigId(info.configId);
        //        MachineMonitor.SetItemId(info.itemId);
        //        MachineMonitor.StartMonitor();

        //        Logger.Info("Post Start Monitor Machine success.");
        //        Logger.Info("startTime:" + startTime);
        //        Logger.Info("endTime:" + endTime);
        //        Logger.Info("RecalUrl:" + info.ReCallUrl);
        //        Logger.Info("ConfigId:" + info.configId);
        //        Logger.Info("ItemId:" + info.itemId);

        //        resp.Data = "0";

        //    } catch (Exception ex) {
        //        resp.StateCode = StateCode.Fail;
        //        resp.Message += ex.ToString();
        //        resp.Data = "1";
        //        Logger.Info("Start Monitor Machine Exception:" + ex);
        //    }

        //    Logger.Info("请求StateCode：" + resp.StateCode + ",Message:" + resp.Message);

        //    String result = info.jsoncallback + "(" + new JavaScriptSerializer().Serialize(resp) + ")";
        //    return result;
        //}


        //public string StopMonitorMachine(string jsoncallback)
        //{
        //    var resp = new APIResponse<string>() {
        //        StateCode = StateCode.Success,
        //        Message = "停止监控成功"
        //    };

        //    try {
        //        MachineMonitor.SetDeadTime(DateTime.Now);
        //        MachineMonitor.StopMonitor();

        //        //Logger.Info("Start Monitor Machine.");
        //        resp.Data = "0";
        //    } catch (Exception ex) {
        //        resp.StateCode = StateCode.Fail;
        //        resp.Message += ex.ToString();
        //        resp.Data = "1";

        //        //Logger.Info("Stop Monitor Machine Exception:" + ex);
        //    }
        //    //return jsoncallback + "(" + new JavaScriptSerializer().Serialize(resp) + ")";
        //}


    }
}
