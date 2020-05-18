using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using WebsiteService.MonitorTerminal.Datas;
using WebsiteService.MonitorTerminal.Monitors;

namespace WebsiteService.MonitorTerminal.Utils
{
    public static class ProcessUtil
    {
        public static List<ProcessInfo> GetProcessInfos()
        {
            var processes = Process.GetProcesses();
            var InstanceNames = ProcessMonitor.GetProcessInstanceNameList();

            List<ProcessMonitor> items = new List<ProcessMonitor>();
            foreach (var process in processes)
            {
                if (process.Id == 0) { continue; }
                if (InstanceNames.Contains(process.ProcessName, StringComparer.OrdinalIgnoreCase))
                {
                    try
                    {
                        ProcessMonitor ProcessMonitor = new ProcessMonitor(process, InstanceNames);
                        ProcessMonitor.Init();
                        items.Add(ProcessMonitor);
                    }
                    catch (System.Exception) { }
                }
            }
            Thread.Sleep(10);
            List<ProcessInfo> processInfos = new List<ProcessInfo>();
            foreach (var item in items)
            {
                try
                {
                    item.UpdateInfo();
                    ProcessInfo info = new ProcessInfo()
                    {
                        Id = item.Id,
                        FileName = item.FileName,
                        ProcessName = item.ProcessName,
                        InstanceName = item.InstanceName,
                        CpuUsage = item.CpuUsage,
                        MemoryUsage = item.MemoryUsage
                    };
                    processInfos.Add(info);
                }
                catch (System.Exception) { }
            }
            processInfos = processInfos.OrderByDescending(q => q.CpuUsage).ToList();
            return processInfos;
        }



        public static List<ProcessInfo> GetProcessInfos2()
        {
            int processorCount = Environment.ProcessorCount;
            List<ProcessInfo> infos = new List<ProcessInfo>();
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
            foreach (ManagementObject mo in searcher2.Get())
            {
                ProcessInfo info = new ProcessInfo();
                info.Id = int.Parse(mo["IDProcess"].ToString());
                if (info.Id == 0) { continue; }
                info.InstanceName = mo["Name"].ToString();
                info.CpuUsage = float.Parse(mo.Properties["PercentProcessorTime"].Value.ToString())/ processorCount;
                info.MemoryUsage = (float) (Convert.ToInt64(mo["WorkingSetPrivate"]) / (1024 * 1024));
                infos.Add(info);
            }
            searcher2.Dispose();

            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.Id == 0) { continue; }
                var info = infos.FirstOrDefault(q => q.Id == process.Id);
                if (info!=null)
                {
                    info.ProcessName = process.ProcessName;
                    try
                    {
                        if (process.MainModule != null)
                        {
                            info.FileName = process.MainModule.FileName;
                        }
                    }
                    catch (Exception) { }
                }
            }

            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process");
            //foreach (ManagementObject mo in searcher.Get())
            //{
            //    ProcessInfo info = new ProcessInfo();
            //    info.FileName = mo["Name"].ToString();
            //    info.Id = int.Parse(mo["ProcessID"].ToString());

            //    ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfProc_Process Where IDProcess=" + info.Id);
            //    foreach (ManagementObject mo2 in searcher2.Get())
            //    {
            //        info.CpuUsage =float.Parse( mo2.Properties["PercentProcessorTime"].Value.ToString());
            //        info.MemoryUsage = (float)(Convert.ToInt64(mo2["VirtualBytes"]) / (10 * 1024 * 1024));
            //        //IDProcess
            //        //Name,PercentProcessorTime 
            //    }
            //    infos.Add(info);
            //    searcher2.Dispose();
            //}
            return infos;

        }



    }
}
