using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using Microsoft.Win32;
using Topshelf;

namespace ServerMonitor
{
    internal class Program
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
                        if ((int) cnt.RawValue == pid)
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

        public class SoftwareInfo
        {
            public string Name { get; set; }//DisplayName
            public string Icon { get; set; }//DisplayIcon
            public string Version { get; set; }//DisplayVersion
            public string InstallLocation { get; set; } //InstallLocation  InstallDir 
            public string Publisher { get; set; }//Publisher   Microsoft Corporation
            public string InstallDate { get; set; }//InstallDate 20191122

        }

        private static void Main(string[] args)
        {
            var archive = ArchiveFactory.Create(SharpCompress.Common.ArchiveType.Zip);
            archive.AddAllFromDirectory(path);

            archive.SaveTo(Path.Combine(Path.GetDirectoryName(path), fileName), new SharpCompress.Writers.WriterOptions(SharpCompress.Common.CompressionType.Deflate)
            {

            });
            archive.Dispose();

            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\Tds\tcp");
            var pn =(int) key.GetValue("PortNumber");
            key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp");
            pn = (int) key.GetValue("PortNumber");
            var kid = key.GetValueKind("PortNumber");
            var name = key.GetValueNames();
            // mstsc
            List<SoftwareInfo> softwareInfos = new List<SoftwareInfo>();
            ShowAllSoftwaresName(softwareInfos);
        }


        public static void DisplayInstalledApps(RegistryKey key, List<SoftwareInfo> softwareInfos)
        {
            string displayName;
            if (key != null)
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    if (displayName == null) { continue; }
                    if (softwareInfos.Any(q => q.Name == displayName)) continue;
                    SoftwareInfo info = new SoftwareInfo();
                    softwareInfos.Add(info);

                    info.Name = displayName;
                    info.Icon = subkey.GetValue("DisplayIcon") as string;
                    info.Version = subkey.GetValue("DisplayVersion") as string;
                    info.Publisher = subkey.GetValue("Publisher") as string;
                    info.InstallDate = subkey.GetValue("InstallDate") as string;
                    info.InstallLocation = subkey.GetValue("InstallLocation") as string ?? subkey.GetValue("InstallDir") as string;
                    if (info.Icon != null && info.Icon.StartsWith("\""))
                    {
                        info.Icon = info.Icon.Trim('"');
                    }
                }
            }
        }

        public static void ShowAllSoftwaresName(List<SoftwareInfo> softwareInfos)
        {
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            DisplayInstalledApps(key, softwareInfos);
            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            DisplayInstalledApps(key, softwareInfos);
            // search in: CurrentUser_64
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            DisplayInstalledApps(key, softwareInfos);
            // search in: LocalMachine_64
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            DisplayInstalledApps(key, softwareInfos);
        }
    }
    internal class Program2
    {

        //需要引用Microsoft.Win32;命名空间 写的有点麻烦
        private void Form1_Load(object sender, EventArgs e)
        {
            //listview1.Checkbox = true;//就是listview每行前面有个复选框, 这个属性名我忘了具体怎么写的了
            //listview1.View = View.Detials;
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall 此键的子健为本机所有注册过的软件的卸载程序,通过此思路进行遍历安装的软件
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            string[] key1 = key.GetSubKeyNames();//返回此键所有的7a686964616fe78988e69d8331333264643731子键名称
            List<string> key2 = key1.ToList<string>();//因为有的项木有"DisplayName"或"DisplayName"的键值的时候要把键值所在数组中的的元素进行删除
            RegistryKey subkey = null;
            try
            {
                for (int i = 0; i < key2.Count; i++)
                {
                    subkey = key.OpenSubKey(key2[i]);//通过list泛型数组进行遍历,某款软件项下的子键
                    if (subkey.GetValue("DisplayName") != null)
                    {
                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            string path = subkey.GetValue("DisplayIcon").ToString();
                            string SubPath = path.Substring(path.Length - 1, 1);//截取子键值的最后一位进行判断
                            if (SubPath == "o" || path.IndexOf("exe") == -1)//如果为o 就是ico 或 找不到exe的 表示为图标文件或只有个标识而没有地址的
                            {
                                key2.RemoveAt(i);//首先删除数组中此索引的元素
                                i -= 1;//把循环条件i的值进行从新复制,否则下面给listview的项的tag属性进行赋值的时候会报错
                                continue;
                            }
                            //listView1.Items.Add(subkey.GetValue("DisplayName").ToString());//把软件名称添加到listview控件中
                            //if (SubPath == "e")//如果为e 就代表着是exe可执行文件,
                            //{
                            //    listView1.Items[i].Tag = path;//则表示可以直接把地址赋给tag属性
                            //    continue;
                            //}
                            //if (SubPath == "0" || SubPath == "1")//因为根据观察 取的是DisplayIcon的值 表示为图片所在路径 如果为0或1,则是为可执行文件的图标
                            //{
                            //    path = path.Substring(0, path.LastIndexOf("e") + 1);//进行字符串截取,
                            //    listView1.Items[i].Tag = path;//则表示可以直接把地址赋给tag属性
                            //    continue;
                            //}

                        }
                        else
                        {
                            key2.RemoveAt(i);//首先删除数组中此索引的元素
                            i -= 1;//把循环条件i的值进行从新复制,否则下面给listview的项的tag属性进行复制的时候会报错
                            continue;
                        }
                    }
                    else
                    {
                        key2.RemoveAt(i);//首先删除数组中此索引的元素
                        i = i - 1;//把循环条件i的值进行从新复制,否则下面给listview的项的tag属性进行复制的时候会报错
                    }
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }



        //private void btn_start_Click(object sender, EventArgs e)
        //{

        //    for (int i = 0; i < listView1.Items.Count; i++)
        //    {
        //        if (listView1.Items[i].Checked)
        //        {
        //            System.Diagnostics.Process.Start(listView1.Items[i].Tag.ToString());//启动所选中的软件
        //        }
        //    }
        //}
    }
}
