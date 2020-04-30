using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WebsiteService.MonitorTerminal.Utils
{
    public static class MstscUtil
    {
        public static int GetPort()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\Tds\tcp");
            var pn = key.GetValue("PortNumber");
            if (pn != null)
            {
                return (int) pn;
            }
            key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp");
            pn = key.GetValue("PortNumber");
            if (pn != null)
            {
                return (int) pn;
            }
            return 3389;
        }

        public static void SetPort(int port)
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\Tds\tcp", true);
            key.SetValue("PortNumber", port);
            key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", true);
            key.SetValue("PortNumber", port);
        }


    }
}
