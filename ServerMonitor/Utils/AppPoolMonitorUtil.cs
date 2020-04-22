//using ServerMonitor.Monitors;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ServerMonitor.Utils
//{
//    public class AppPoolMonitorUtil
//    {
//        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//        private static Hashtable monitorHT = new Hashtable(); //创建一个Hashtable实例
//        #region 初始化
//        /// <summary>
//        /// 初始化程序池监控
//        /// </summary>
//        /// <param name="configs"></param>
//        public static void InitAppPoolMonitor(List<string> poolNames)
//        {
//            //Logger.Info("初始化程序池监控信息-start");
//            int counter = ProcessorCountMonitor.getValue();
//            //Logger.Info("counter:" + counter);
//            List<AppPoolMonitorInfo> poolMonitors = new List<AppPoolMonitorInfo>();
//            ArrayList listThread = new ArrayList();
//            DateTime d2 = DateTime.Now;
//            // 获取所有程序池名称
//            DateTime d3 = DateTime.Now;
//            Process[] ps = Process.GetProcessesByName("w3wp");
//            DateTime d4 = DateTime.Now;
//            //Logger.Info("需监控pool如下:");
//            //foreach (string name in poolNames) {
//            //    Logger.Info(name);
//            //}
//            foreach (Process p in ps) {
//                string name = MachineMonitor.GetProcessUserName(p.Id);
//                //Logger.Info("Process Name:"+name);
//                if (poolNames.IndexOf(name) >= 0 && !monitorHT.ContainsKey(name)) {
//                    //Logger.Info("Name:" + name + "未生成监控，新建线程生成监控");
//                    Thread parameterThread = new Thread(delegate () { GetSingleAppPoolMonitor(name, p, counter, ref monitorHT); });
//                    parameterThread.Name = name;
//                    parameterThread.Start();
//                    //Logger.Info("Thread " + name+" start");
//                    listThread.Add(parameterThread);
//                }
//            }
//            foreach (Thread thread in listThread) {
//                thread.Join();
//            }
//            //Logger.Info("实例化的监控信息如下：");
//            int n = monitorHT.Count;
//            //foreach (DictionaryEntry de in monitorHT) //ht为一个Hashtable实例
//            //{
//            //    Logger.Info("key:" + de.Key);//de.Key对应于keyvalue键值对key
//            //    Logger.Info("Value:" + de.Value);//de.Key对应于keyvalue键值对value
//            //}
//            //Logger.Info("初始化程序池监控信息-Finish");
//        }
//        public static void GetSingleAppPoolMonitor(string name, Process p, int counter, ref Hashtable ht)
//        {
//            try {
//                //Logger.Info("in GetAppPoolInfo()");
//                AppPoolMonitor info = new AppPoolMonitor();
//                info.name = name;
//                info.count = counter;
//                info.process = p;
//                //Logger.Info("p.ProcessName:" + p.ProcessName);
//                //Logger.Info("p.ProcessName:" + p.Id);
//                // 获取CPU占用率 PerformanceCounter         
//                string instance1 = GetInstanceName("Process", "ID Process", p);
//                if (instance1 != null) {
//                    PerformanceCounter cpucounter = new PerformanceCounter("Process", "% Processor Time", instance1);
//                    if (cpucounter != null) {
//                        cpucounter.NextValue();
//                        System.Threading.Thread.Sleep(200); //等200ms(是测出能换取下个样本的最小时间间隔)，让后系统获取下一个样本
//                        info.cpuCounter = cpucounter;
//                        //Logger.Info("Name:" + name + "生成CPU监控成功" + instance1);
//                    } else {
//                        //Logger.Info("Name:" + name + "生成CPU监控失败" + instance1);
//                    }
//                } else {
//                    //Logger.Info("Name:" + name + "获取cpu监控实例失败" + instance1);
//                }
//                // 获取GC占用率 PerformanceCounter            
//                string instance2 = GetInstanceName(".NET CLR Memory", "Process ID", p);
//                if (instance2 != null) {
//                    PerformanceCounter gccounter = new PerformanceCounter(".NET CLR Memory", "% Time in GC", instance2);
//                    if (gccounter != null) {
//                        gccounter.NextValue();
//                        info.gcCounter = gccounter;
//                        //Logger.Info("Name:" + name + "生成GC监控成功" + instance2);
//                    } else {
//                        //Logger.Info("Name:" + name + "生成GC监控失败" + instance2);
//                    }
//                } else {
//                    //Logger.Info("Name:" + name + "获取GC监控实例失败" + instance2);
//                }
//                lock (ht) {
//                    // Access thread-sensitive resources.
//                    if (info != null) {
//                        ht.Add(name, info);
//                        //Logger.Info("Add name:" + name + " into hashtable");
//                    }
//                }
//            } catch (Exception ex) {
//                //Logger.Info("Exception:" + ex.ToString());
//            }
//        }
//        public static string GetInstanceName(string categoryName, string counterName, Process p)
//        {
//            try {
//                PerformanceCounterCategory processcounter = new PerformanceCounterCategory(categoryName);
//                string[] instances = processcounter.GetInstanceNames();
//                foreach (string instance in instances) {
//                    PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instance);
//                    //Logger.Info("对比in mothod GetInstanceName，" + counter.NextValue() + "：" + p.Id);
//                    if (counter.NextValue() == p.Id) {
//                        return instance;
//                    }
//                }
//            } catch (Exception ex) {
//            }
//            return null;
//        }
//        #endregion
//        public static float GetAppPoolMonitorCpu(string poolName)
//        {
//            //Logger.Info("In Method GetAppPoolMonitorCpu(string poolName) and poolName is " + poolName);
//            PerformanceCounter cpuCounter = ((AppPoolMonitor)monitorHT[poolName]).cpuCounter;
//            if (cpuCounter != null) {
//                return cpuCounter.NextValue() / ((AppPoolMonitor)monitorHT[poolName]).count;
//            }
//            return 0;
//        }
//        public static float GetAppPoolMonitorMemory(string poolName)
//        {
//            Process process = ((AppPoolMonitor)monitorHT[poolName]).process;
//            return process.WorkingSet64 / (1024 * 1024);
//        }
//        public static float GetAppPoolMonitorGC(string poolName)
//        {
//            PerformanceCounter gcCounter = ((AppPoolMonitor)monitorHT[poolName]).gcCounter;
//            if (gcCounter != null) {
//                return gcCounter.NextValue();
//            }
//            return 0;
//        }
//    }
//    public class AppPoolMonitor
//    {
//        public string name { get; set; } //程序池名
//        public int count { get; set; } //核数
//        public PerformanceCounter cpuCounter { get; set; }//cpu占用率：( cpuCounter.NextValue()/count).ToString("F");
//        public PerformanceCounter gcCounter { get; set; }
//        public Process process { get; set; }//内存：(process.WorkingSet64 / (1024 * 1024)).ToString("F");
//    }
//}
