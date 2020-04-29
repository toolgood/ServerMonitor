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
    public class ProcessUtil
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


        //public struct ProInfo
        //{
        //    //进程名 
        //    public string stringName;

        //    //进程ID 
        //    public string stringPID;

        //    //父进程ID 
        //    public string stringFID;

        //    //用户名 
        //    public string stringUser;

        //    //CPU占用率 
        //    public string stringCPU;

        //    //RAM占用量 
        //    public string stringRAM;
        //}

 
        //private static ManagementObjectCollection ExecSearch(string command)
        //{
        //    ManagementObjectCollection moc = null;
        //    //ObjectQuery objQuery = new ObjectQuery(command);
        //    //string stringMachineName = "localhost";
        //    //ManagementScope scope = new ManagementScope("\\\\" + stringMachineName + "\\root\\cimv2");
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_PerfFormattedData_PerfProc_Process");
        //    //ManagementObjectSearcher searcher = new ManagementObjectSearcher( objQuery);
        //    try
        //    {
        //        moc = searcher.Get();
        //    }
        //    catch (Exception x)
        //    {
        //        //MessageBox.Show("Error:" + x.Message);
        //    }
        //    return moc;
        //}

        //public static List<ProcessInfo> GetProcessInfos2()
        //{
        //    ManagementObjectCollection moc = null;
        //    ManagementObjectCollection moc1 = null;
        //    var processes = Process.GetProcesses().ToList();
        //    processes.RemoveAll(q => q.Id == 0);
        //    var pro = processes.Select(q => q.ProcessName).ToList();

        //    int pnum = 0;
        //    ProInfo[] proInfomation;
        //    proInfomation = new ProInfo[pro.Count];
        //    foreach (string p in pro)
        //    {
        //        pnum++;

        //        string selstr = "SELECT * FROM Win32_Process Where Name='" + p + "'";
        //        moc = ExecSearch(selstr);


        //        foreach (ManagementObject mo in moc)
        //        {
        //            proInfomation[pnum].stringName = mo["Name"].ToString();
        //            proInfomation[pnum].stringPID = mo["ProcessID"].ToString();
        //            proInfomation[pnum].stringFID = mo["ParentProcessID"].ToString();

        //            string selstr2 = "SELECT * FROM Win32_PerfFormattedData_PerfProc_Process Where IDProcess=" + proInfomation[pnum].stringPID;
        //            moc1 = ExecSearch(selstr2);

        //            if (moc1 == null)
        //            {
        //                continue;
        //            }


        //            foreach (ManagementObject mo1 in moc1)
        //            {
        //                proInfomation[pnum].stringCPU = mo1.Properties["PercentProcessorTime"].Value.ToString();
        //                proInfomation[pnum].stringRAM = (Convert.ToInt32(mo1["VirtualBytes"]) / (10 * 1024 * 1024)).ToString();
        //            }

        //        }
        //    }
        //    return null;
        //}



    }
}
