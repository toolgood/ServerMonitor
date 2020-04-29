using System;
using System.Diagnostics;
using System.Linq;

namespace WebsiteService.MonitorTerminal.Monitors
{
    public class ProcessMonitor
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
        /// <summary>s
        /// CUP使用量 %
        /// </summary>
        public float CpuUsage { get; private set; }


        public ProcessMonitor(Process process)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            try
            {
                if (process.MainModule!=null)
                {
                    FileName = process.MainModule.FileName;
                }
            }
            catch (Exception) { }
            InstanceName = GetProcessInstanceName(process);
            WorkingSetPrivate = new PerformanceCounter("Process", "Working Set - Private", InstanceName, true);
            ProcessorTime = new PerformanceCounter("Process", "% Processor Time", InstanceName, true);
        }

        public ProcessMonitor(Process process, string[] instances)
        {
            Id = process.Id;
            ProcessName = process.ProcessName;
            try
            {
                FileName = process.MainModule.FileName;
            }
            catch (Exception) { }
            InstanceName = GetProcessInstanceName(instances, process);
            WorkingSetPrivate = new PerformanceCounter("Process", "Working Set - Private", InstanceName, true);
            ProcessorTime = new PerformanceCounter("Process", "% Processor Time", InstanceName, true);
        }


        public static string[] GetProcessInstanceNameList()
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
            var instances = cat.GetInstanceNames();
            return instances;
        }
        private string GetProcessInstanceName(Process process)
        {
            var instances = GetProcessInstanceNameList();
            return GetProcessInstanceName(instances, process);
        }

        private string GetProcessInstanceName(string[] instances2, Process process)
        {
            var instances = instances2.ToList();
            instances.RemoveAll(q => q.StartsWith(process.ProcessName, StringComparison.CurrentCultureIgnoreCase) == false);
            if (instances.Count == 1)
            {
                return instances[0];
            }
            foreach (string instance in instances)
            {
                if (instance.StartsWith(process.ProcessName, StringComparison.CurrentCultureIgnoreCase))
                {
                    using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
                    {
                        if ((int) cnt.RawValue == process.Id)
                        {
                            return instance;
                        }
                    }
                }
            }
            foreach (string instance in instances)
            {
                if (instance == process.ProcessName)
                {
                    return instance;
                }
            }
            throw new Exception("Could not find performance counter instance name for current process. This is truly strange ...");
        }



        public void Init()
        {
            //if (WorkingSetPrivate != null)
            //{
            //    WorkingSetPrivate.NextValue();
            //}
            if (ProcessorTime != null)
            {
                ProcessorTime.NextValue();
            }
        }
        public void UpdateInfo()
        {
            if (WorkingSetPrivate != null)
            {
                MemoryUsage = WorkingSetPrivate.NextValue() / 1024 / 1024;
            }
            if (ProcessorTime != null)
            {
                CpuUsage = ProcessorTime.NextValue() / Environment.ProcessorCount;
            }
        }

        public void Dispose()
        {
            ProcessName = null;
            InstanceName = null;
            if (WorkingSetPrivate != null)
            {
                WorkingSetPrivate.Dispose();
            }
            WorkingSetPrivate = null;
            if (ProcessorTime != null)
            {
                ProcessorTime.Dispose();
            }
            ProcessorTime = null;
        }
    }
}
