﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebsiteServiceClient.Monitors
{
    public class MemoryMonitor
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength;
            public uint dwMemoryLoad;//内存占用比
            public UInt64 dwTotalPhys; //总的物理内存大小
            public UInt64 dwAvailPhys; //可用的物理内存大小 
            public UInt64 dwTotalPageFile;
            public UInt64 dwAvailPageFile; //可用的页面文件大小
            public UInt64 dwTotalVirtual; //返回调用进程的用户模式部分的全部可用虚拟地址空间
            public UInt64 dwAvailVirtual; // 返回调用进程的用户模式部分的实际自由可用的虚拟地址空间
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }
        /// <summary>
        /// 存放内存信息
        /// </summary>
        public class MemoryInfo
        {
            public uint MemoryLoad { get; set; }//返回00形式
            public ulong TotalPhys { get; set; } //以Bite为单位
            public ulong AvailPhys { get; set; }//以Bite为单位
        }


        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <param name="meminfo"></param>
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        [DllImport("kernel32")]
        public static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX stat);
        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static MemoryInfo GetMemoryInfo()
        {
            MEMORY_INFO memInfo = new MEMORY_INFO();
            MEMORYSTATUSEX memEx = new MEMORYSTATUSEX();
            memEx.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            GlobalMemoryStatusEx(ref memEx);
            GlobalMemoryStatus(ref memInfo);
            MemoryInfo memoryInfo = new MemoryInfo();
            memoryInfo.MemoryLoad = memInfo.dwMemoryLoad;
            memoryInfo.AvailPhys = memInfo.dwAvailPhys;
            memoryInfo.TotalPhys = memInfo.dwTotalPhys;
            return memoryInfo;
        }
        /// <summary>
        /// 获取内存占用率
        /// </summary>
        /// <returns></returns>
        public static uint GetMenoryLoad()
        {
            MEMORY_INFO memInfo = new MEMORY_INFO();
            MEMORYSTATUSEX memEx = new MEMORYSTATUSEX();
            memEx.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            GlobalMemoryStatusEx(ref memEx);
            GlobalMemoryStatus(ref memInfo);
            return memInfo.dwMemoryLoad;
        }
    }
}
