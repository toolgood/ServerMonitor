using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using WebsiteService.MonitorTerminal.Datas;

namespace WebsiteService.MonitorTerminal.Utils
{
    public static class SoftwareUtil
    {
        public static List<SoftwareInfo> GetSoftwareInfos()
        {
            List<SoftwareInfo> softwareInfos = new List<SoftwareInfo>();

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


            softwareInfos = softwareInfos.OrderBy(q => q.Name).ToList();
            return softwareInfos;
        }

        private static void DisplayInstalledApps(RegistryKey key, List<SoftwareInfo> softwareInfos)
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
                    if (string.IsNullOrWhiteSpace(info.InstallLocation))
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(info.Icon) && softwareInfos.Any(q => info.InstallLocation==q.InstallLocation && info.Publisher == q.Publisher))
                    {
                        continue;
                    }
                    //if (string.IsNullOrWhiteSpace(info.Icon) && softwareInfos.Any(q => info.InstallLocation.StartsWith(q.InstallLocation) && info.Publisher == q.Publisher))
                    //{
                    //    continue;
                    //}
                    //if (string.IsNullOrWhiteSpace(info.Icon) == false)
                    //{
                    //    softwareInfos.RemoveAll(q => q.InstallLocation == info.InstallLocation
                    //                && info.Publisher == q.Publisher
                    //                && string.IsNullOrWhiteSpace(q.Icon));
                    //}
                    softwareInfos.Add(info);

                }
            }
        }


    }
}
