using ServerMonitor.Datas;
using ServerMonitor.Monitors;
using ServerMonitor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Topshelf;

namespace ServerMonitor
{
    class Program
    {
        public class ProcessMonitorItem : IDisposable
        {
            public int Id { get; set; }
            public string ProcessName { get; private set; }
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
                InstanceName = GetProcessInstanceName(process.Id);
                WorkingSetPrivate = new PerformanceCounter("Process", "Working Set - Private", InstanceName, true);
                ProcessorTime = new PerformanceCounter("Process", "% Processor Time", InstanceName, true);
            }
            private static string GetProcessInstanceName(int pid)
            {
                PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
                string[] instances = cat.GetInstanceNames();
                foreach (string instance in instances)
                {
                    using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
                    {
                        if ((int)cnt.RawValue == pid)
                        {
                            return instance;
                        }
                    }
                }
                throw new Exception("Could not find performance counter instance name for current process. This is truly strange ...");
            }
 
            public void Init()
            {
                if (WorkingSetPrivate != null)
                {
                    WorkingSetPrivate.NextValue();
                }
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

        static void Main(string[] args)
        {
            //获取当前进程对象
            Process cur = Process.GetCurrentProcess();
            ProcessMonitorItem processMonitor = new ProcessMonitorItem(cur);
            processMonitor.Init();
            Thread.Sleep(200);
            while (true)
            {
                processMonitor.UpdateInfo();
                Console.WriteLine("{0}:{1}  {2:N}MB CPU使用率：{3}%", processMonitor.ProcessName, "私有工作集    ", processMonitor.MemoryUsage, processMonitor.CpuUsage);


                Thread.Sleep(1000);
            }


            PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);

            PerformanceCounter curpc = new PerformanceCounter("Process", "Working Set", cur.ProcessName);
            PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

            //上次记录CPU的时间
            TimeSpan prevCpuTime = TimeSpan.Zero;
            //Sleep的时间间隔
            int interval = 1000;

            PerformanceCounter totalcpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            //SystemInfo sys = new SystemInfo();
            const int KB_DIV = 1024;
            const int MB_DIV = 1024 * 1024;
            const int GB_DIV = 1024 * 1024 * 1024;
            while (true)
            {
                //第一种方法计算CPU使用率
                //当前时间
                TimeSpan curCpuTime = cur.TotalProcessorTime;
                //计算
                double value = (curCpuTime - prevCpuTime).TotalMilliseconds / interval / Environment.ProcessorCount * 100;
                prevCpuTime = curCpuTime;

                Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集(进程类)", cur.WorkingSet64 / 1024, value);//这个工作集只是在一开始初始化，后期不变
                Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}", cur.ProcessName, "工作集        ", curpc.NextValue() / 1024, value);//这个工作集是动态更新的
                //第二种计算CPU使用率的方法
                Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024, curtime.NextValue() / Environment.ProcessorCount);
                //Thread.Sleep(interval);

                //第一种方法获取系统CPU使用情况
                Console.Write("\r系统CPU使用率：{0}%", totalcpu.NextValue());
                //Thread.Sleep(interval);

                //第二章方法获取系统CPU和内存使用情况
                //Console.Write("\r系统CPU使用率：{0}%，系统内存使用大小：{1}MB({2}GB)", sys.CpuLoad, (sys.PhysicalMemory - sys.MemoryAvailable) / MB_DIV, (sys.PhysicalMemory - sys.MemoryAvailable) / (double)GB_DIV);
                Thread.Sleep(interval);
            }

            Console.ReadLine();


            var rc = HostFactory.Run(x => {
                x.Service<MainService>(s => {
                    s.ConstructUsing(name => new MainService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("ServerMonitor");
                x.SetDisplayName("ServerMonitor");
                x.SetServiceName("ServerMonitor");
            });
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
