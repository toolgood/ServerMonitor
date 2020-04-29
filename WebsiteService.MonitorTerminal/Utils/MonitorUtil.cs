using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebsiteService.MonitorTerminal.Datas;
using WebsiteService.MonitorTerminal.Monitors;

namespace WebsiteService.MonitorTerminal.Utils
{
    public class MonitorUtil
    {
        #region 获取机器信息
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
            //foreach (Thread thread in listThread) {
            //    thread.Abort();
            //}
            return info;
        }

        private static void GetCpu(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.CpuUsage = CPUMonitor.GetValue();
            }
        }
        private static void GetCpuCount(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.CoreNumber = ProcessorCountMonitor.GetValue();
            }
        }
        private static void GetMenoryAvaliable(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.MemoryAvailable = (MemoryMonitor.GetMemoryInfo().AvailPhys / (1024 * 1024 * 1024));
            }
        }
        private static void GetMenoryTotal(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.PhysicalMemory = (MemoryMonitor.GetMemoryInfo().TotalPhys / (1024 * 1024 * 1024));
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
        private static void GetDiskRead(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.DiskReadData = (int)((DiskReadMonitor.GetValue()) / 1024);
            }
        }
        private static void GetDiskWrite(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.DiskWriteData = (int)((DiskWriteMonitor.GetValue()) / 1024);
            }
        }
        private static void GetNetworkReceive(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.NetworkReceiveData = (int)((NetworkReceiveMonitor.GetValue()) / 1024);
            }
        }
        private static void GetNetworkSend(ref MachineMonitorInfo info)
        {
            lock (info) {
                info.NetworkSendData = (int)((NetworkSendMonitor.GetValue()) / 1024);
            }
        }
        #endregion
    }
}
