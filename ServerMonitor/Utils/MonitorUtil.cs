using ServerMonitor.Monitors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerMonitor.Utils
{
    public class MonitorUtil
    {
        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static MachineMonitorInfo GetMachineMonitorInfo()
        {
            MachineMonitorInfo info = new MachineMonitorInfo();
            ArrayList listThread = new ArrayList();
            // 获取CPU占用比
            Thread thread1 = new Thread(delegate () { GetCpu(ref info); });
            thread1.Start();
            listThread.Add(thread1);
            // 获取CPU核数
            Thread thread2 = new Thread(delegate () { GetCpuCount(ref info); });
            thread2.Start();
            listThread.Add(thread2);
            // 获取可用内存
            Thread thread3 = new Thread(delegate () { GetMenoryAvaliable(ref info); });
            thread3.Start();
            listThread.Add(thread3);
            // 获取总内存
            Thread thread4 = new Thread(delegate () { GetMenoryTotal(ref info); });
            thread4.Start();
            listThread.Add(thread4);
            // 获取 硬盘空间
            Thread thread51 = new Thread(delegate () { GetHardDisk(ref info); });
            thread51.Start();
            listThread.Add(thread51);

            // 获取Disk Read
            Thread thread5 = new Thread(delegate () { GetDiskRead(ref info); });
            thread5.Start();
            listThread.Add(thread5);

            // 获取Disk Write
            Thread thread6 = new Thread(delegate () { GetDiskWrite(ref info); });
            thread6.Start();
            listThread.Add(thread6);

            // 获取Network Receive
            Thread thread7 = new Thread(delegate () { GetNetworkReceive(ref info); });
            thread7.Start();
            listThread.Add(thread7);

            // 获取Network Send
            Thread thread8 = new Thread(delegate () { GetNetworkSend(ref info); });
            thread8.Start();
            listThread.Add(thread8);


            foreach (Thread thread in listThread) {
                thread.Join();
            }
            foreach (Thread thread in listThread) {
                thread.Abort();
            }
            return info;
        }
 

        private static void GetCpu(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.CpuUsage = CPUMonitor.getValue();
            }
        }
        private static void GetCpuCount(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.CoreNumber = ProcessorCountMonitor.getValue();
            }
        }
        private static void GetMenoryAvaliable(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.MemoryAvailable = (MemoryMonitor.getMemoryInfo().availPhys / (1024 * 1024 * 1024));
            }
        }
        private static void GetMenoryTotal(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.PhysicalMemory = (MemoryMonitor.getMemoryInfo().totalPhys / (1024 * 1024 * 1024));
            }
        }

        private static void GetHardDisk(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.HardDisks = new List<HardDiskInfo>();

                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives) {
                    if (drive.IsReady == false) { continue; }
                    if (drive.DriveType == System.IO.DriveType.Removable) { continue; }
                    if (drive.DriveType == System.IO.DriveType.CDRom) { continue; }
                    if (drive.DriveType == System.IO.DriveType.Ram) { continue; }
                    if (drive.DriveType == System.IO.DriveType.NoRootDirectory) { continue; }

                    HardDiskInfo hardDisk = new HardDiskInfo();
                    hardDisk.Name = drive.Name.Replace(":\\", "");
                    hardDisk.Space = (float)(drive.TotalSize / (1024 * 1024 * 1024.0));
                    hardDisk.FreeSpace = (float)(drive.TotalFreeSpace / (1024 * 1024 * 1024.0));

                    info.HardDisks.Add(hardDisk);
                }
            }
        }
        // 单kb
        public static void GetDiskRead(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.diskReadData = (int)((DiskReadMonitor.getValue()) / 1024);
            }
        }

        // 单位Kb
        public static void GetDiskWrite(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.diskWriteData = (int)((DiskWriteMonitor.getValue()) / 1024);
            }
        }
        // 单位Kb
        public static void GetNetworkReceive(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.networkReceiveData = (int)((NetworkReceiveMonitor.getValue()) / 1024);
            }
        }

        // 单位Kb
        public static void GetNetworkSend(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.networkSendData = (int)((NetworkSendMonitor.getValue()) / 1024);
            }
        }

    }
 

    [Serializable]
    public class MachineMonitorInfo
    {
        /// <summary>
        /// CPU 使用率
        /// </summary>
        public float CpuUsage { get; set; }
        /// <summary>
        /// 核心数
        /// </summary>
        public int CoreNumber { get; set; }
        /// <summary>
        /// 可用内存 MB
        /// </summary>
        public float MemoryAvailable { get; set; }
        /// <summary>
        /// 物理内存 MB
        /// </summary>
        public float PhysicalMemory { get; set; }

        public List<HardDiskInfo> HardDisks { get; set; }

        public int diskReadData { get; set; }
        public int diskWriteData { get; set; }
        public int networkReceiveData { get; set; }
        public int networkSendData { get; set; }

        public override string ToString()
        {
            return $"CPU:{CoreNumber}核，使用率{CpuUsage:0.00}%，可用内存{MemoryAvailable:0.00}GB，总共内存{PhysicalMemory:0.00}GB";
        }

    }
    public class HardDiskInfo
    {
        public string Name { get; set; }
        /// <summary>
        /// 硬盘可用空间 MB
        /// </summary>
        public float FreeSpace { get; set; }

        /// <summary>
        /// 硬盘空间 MB
        /// </summary>
        public float Space { get; set; }

        public override string ToString()
        {
            return $"{Name}盘,可用{FreeSpace:0.00}GB,总共{Space:0.00}GB";
        }
    }


}
