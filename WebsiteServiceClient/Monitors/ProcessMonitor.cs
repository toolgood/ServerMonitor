using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using WebsiteServiceClient.Datas;
using System.Threading;

namespace WebsiteServiceClient.Monitors
{
    public class ProcessMonitor
    {
        #region ProcessMonitorItem
        public class ProcessMonitorItem : IDisposable
        {
            public int Id { get; set; }
            public string ProcessName { get; private set; }
            public string FileName { get; private set; }
            public string InstanceName { get; private set; }
            private PerformanceCounter WorkingSetPrivate;
            private PerformanceCounter ProcessorTime;
            /// <summary>
            /// 内存使用量 MB
            /// </summary>
            public float MemoryUsage { get; private set; }
            /// <summary>
            /// CUP使用量 %
            /// </summary>
            public float CpuUsage { get; private set; }


            public ProcessMonitorItem(Process process)
            {
                Id = process.Id;
                ProcessName = process.ProcessName;
                FileName = process.MainModule.FileName;
                InstanceName = GetProcessInstanceName(process.Id);
                WorkingSetPrivate = new PerformanceCounter("Process", "Working Set - Private", InstanceName, true);
                ProcessorTime = new PerformanceCounter("Process", "% Processor Time", InstanceName, true);
            }
            private static string GetProcessInstanceName(int pid)
            {
                PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
                string[] instances = cat.GetInstanceNames();
                foreach (string instance in instances) {
                    using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true)) {
                        if ((int)cnt.RawValue == pid) {
                            return instance;
                        }
                    }
                }
                throw new Exception("Could not find performance counter instance name for current process. This is truly strange ...");
            }

            public void Init()
            {
                if (WorkingSetPrivate != null) {
                    WorkingSetPrivate.NextValue();
                }
                if (ProcessorTime != null) {
                    ProcessorTime.NextValue();
                }
            }
            public void UpdateInfo()
            {
                if (WorkingSetPrivate != null) {
                    MemoryUsage = WorkingSetPrivate.NextValue() / 1024 / 1024;
                }
                if (ProcessorTime != null) {
                    CpuUsage = ProcessorTime.NextValue() / Environment.ProcessorCount;
                }
            }

            public void Dispose()
            {
                ProcessName = null;
                InstanceName = null;
                if (WorkingSetPrivate != null) {
                    WorkingSetPrivate.Dispose();
                }
                WorkingSetPrivate = null;
                if (ProcessorTime != null) {
                    ProcessorTime.Dispose();
                }
                ProcessorTime = null;
            }
        }

        #endregion

        public static List<ProcessInfo> GetProcessInfos()
        {
            var processes = Process.GetProcesses();
            List<ProcessMonitorItem> items = new List<ProcessMonitorItem>();
            foreach (var process in processes) {
                ProcessMonitorItem processMonitorItem = new ProcessMonitorItem(process);
                processMonitorItem.Init();
                items.Add(processMonitorItem);
            }
            Thread.Sleep(200);
            List<ProcessInfo> processInfos = new List<ProcessInfo>();
            foreach (var item in items) {
                item.UpdateInfo();
                ProcessInfo info = new ProcessInfo() {
                    Id = item.Id,
                    FileName = item.FileName,
                    ProcessName = item.ProcessName,
                    InstanceName = item.InstanceName,
                    CpuUsage = item.CpuUsage,
                    MemoryUsage = item.MemoryUsage
                };
                processInfos.Add(info);
            }
            processInfos = processInfos.OrderByDescending(q => q.CpuUsage).ToList();
            return processInfos;
        }


    }
}
