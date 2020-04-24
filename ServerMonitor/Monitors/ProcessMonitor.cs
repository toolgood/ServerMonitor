using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Monitors
{
    public class ProcessMonitor
    {
        public class ProcessInfo
        {
            // Token: 0x04000023 RID: 35
            public string Name;

            // Token: 0x04000024 RID: 36
            public string CpuUsage;

            // Token: 0x04000025 RID: 37
            public int ID;

            // Token: 0x04000026 RID: 38
            public long OldCpuUsage;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00002E71 File Offset: 0x00001E71
        public static void Init()
        {
            ProcessMonitor.ValueFormat.NumberFormat.NumberDecimalDigits = 1;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002E88 File Offset: 0x00001E88
        public static void UpdateProcessList()
        {
            Process[] processes = Process.GetProcesses();
            ProcessMonitor.UpdateCpuUsagePercent(processes);
            ProcessMonitor.UpdateExistingProcesses(processes);
            ProcessMonitor.AddNewProcesses(processes);
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00002EB4 File Offset: 0x00001EB4
        private static void UpdateCpuUsagePercent(Process[] NewProcessList)
        {
            double num = 0.0;
            ProcessMonitor.TotalCpuUsageValue = ProcessMonitor.TotalCpuUsage.NextValue();
            foreach (Process process in NewProcessList)
            {
                if (process.Id != 0)
                {
                    ProcessInfo processInfo = ProcessMonitor.ProcessInfoByID(process.Id);
                    if (processInfo == null)
                    {
                        num += process.TotalProcessorTime.TotalMilliseconds;
                    }
                    else
                    {
                        num += process.TotalProcessorTime.TotalMilliseconds - (double)processInfo.OldCpuUsage;
                    }
                }
            }
            ProcessMonitor.CpuUsagePercent = num / (double)(100f - ProcessMonitor.TotalCpuUsageValue);
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002F6C File Offset: 0x00001F6C
        private static void UpdateExistingProcesses(Process[] NewProcessList)
        {
            if (ProcessMonitor.ProcessList == null)
            {
                ProcessMonitor.ProcessList = new ProcessInfo[NewProcessList.Length];
            }
            else
            {
                ProcessInfo[] array = new ProcessInfo[NewProcessList.Length];
                ProcessMonitor.ProcessIndex = 0;
                foreach (ProcessInfo processInfo in ProcessMonitor.ProcessList)
                {
                    Process process = ProcessMonitor.ProcessExists(NewProcessList, processInfo.ID);
                    if (process == null)
                    {
                        //ProcessMonitor.CallProcessClose(processInfo);
                    }
                    else
                    {
                        array[ProcessMonitor.ProcessIndex++] = ProcessMonitor.GetProcessInfo(processInfo, process);
                        //ProcessMonitor.CallProcessUpdate(processInfo);
                    }
                }
                ProcessMonitor.ProcessList = array;
            }
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00003024 File Offset: 0x00002024
        private static Process ProcessExists(Process[] NewProcessList, int ID)
        {
            foreach (Process process in NewProcessList)
            {
                if (process.Id == ID)
                {
                    return process;
                }
            }
            return null;
        }

        // Token: 0x06000043 RID: 67 RVA: 0x0000306C File Offset: 0x0000206C
        private static ProcessInfo GetProcessInfo(ProcessInfo TempProcess, Process CurrentProcess)
        {
            if (CurrentProcess.Id == 0)
            {
                TempProcess.CpuUsage = ProcessMonitor.TotalCpuUsageValue.ToString("F", ProcessMonitor.ValueFormat);
            }
            else
            {
                long num = (long)CurrentProcess.TotalProcessorTime.TotalMilliseconds;
                TempProcess.CpuUsage = ((double)(num - TempProcess.OldCpuUsage) / ProcessMonitor.CpuUsagePercent).ToString("F", ProcessMonitor.ValueFormat);
                TempProcess.OldCpuUsage = num;
            }
            return TempProcess;
        }

        // Token: 0x06000044 RID: 68 RVA: 0x000030EC File Offset: 0x000020EC
        private static void AddNewProcesses(Process[] NewProcessList)
        {
            foreach (Process newProcess in NewProcessList)
            {
                if (!ProcessMonitor.ProcessInfoExists(newProcess))
                {
                    ProcessMonitor.AddNewProcess(newProcess);
                }
            }
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00003124 File Offset: 0x00002124
        private static bool ProcessInfoExists(Process NewProcess)
        {
            bool result;
            if (ProcessMonitor.ProcessList == null)
            {
                result = false;
            }
            else
            {
                foreach (ProcessInfo processInfo in ProcessMonitor.ProcessList)
                {
                    if (processInfo != null && processInfo.ID == NewProcess.Id)
                    {
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        // Token: 0x06000046 RID: 70 RVA: 0x0000318C File Offset: 0x0000218C
        private static ProcessInfo ProcessInfoByID(int ID)
        {
            ProcessInfo result;
            if (ProcessMonitor.ProcessList == null)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < ProcessMonitor.ProcessList.Length; i++)
                {
                    if (ProcessMonitor.ProcessList[i] != null && ProcessMonitor.ProcessList[i].ID == ID)
                    {
                        return ProcessMonitor.ProcessList[i];
                    }
                }
                result = null;
            }
            return result;
        }

        // Token: 0x06000047 RID: 71 RVA: 0x000031F4 File Offset: 0x000021F4
        private static void AddNewProcess(Process NewProcess)
        {
            ProcessInfo processInfo = new ProcessInfo();
            processInfo.Name = NewProcess.ProcessName;
            processInfo.ID = NewProcess.Id;
            ProcessMonitor.ProcessList[ProcessMonitor.ProcessIndex++] = ProcessMonitor.GetProcessInfo(processInfo, NewProcess);
            //ProcessMonitor.CallNewProcess(processInfo);
        }

        // Token: 0x04000018 RID: 24
        private const Process CLOSED_PROCESS = null;

        // Token: 0x04000019 RID: 25
        private const ProcessInfo PROCESS_INFO_NOT_FOUND = null;

        // Token: 0x0400001D RID: 29
        public static ProcessInfo[] ProcessList;

        // Token: 0x0400001E RID: 30
        public static double CpuUsagePercent;

        // Token: 0x0400001F RID: 31
        private static int ProcessIndex;

        // Token: 0x04000020 RID: 32
        public static CultureInfo ValueFormat = new CultureInfo("en-US");

        // Token: 0x04000021 RID: 33
        private static PerformanceCounter TotalCpuUsage = new PerformanceCounter("Process", "% Processor Time", "Idle");

        // Token: 0x04000022 RID: 34
        private static float TotalCpuUsageValue;
    }
}
