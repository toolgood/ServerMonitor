using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class ServerInfo
    {
        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ServiceType ServiceType { get; }

        public ServiceStartMode StartType { get; }

        public ServiceControllerStatus Status { get; }

        public string SrcFilePath { get; set; }

        //Environment.ExpandEnvironmentVariables(unexpandedPath)
        public string FilePath { get; set; }
    }
    //[Flags]
    //public enum ServiceType2
    //{
    //    KernelDriver = 1,
    //    FileSystemDriver = 2,
    //    Adapter = 4,
    //    RecognizerDriver = 8,
    //    Win32OwnProcess = 16,
    //    Win32ShareProcess = 32,
    //    InteractiveProcess = 256
    //}
    //public enum ServiceStartMode2
    //{
    //    Boot = 0,
    //    System = 1,
    //    Automatic = 2,
    //    Manual = 3,
    //    Disabled = 4
    //}
    //public enum ServiceControllerStatus2
    //{
    //    Stopped = 1,
    //    StartPending = 2,
    //    StopPending = 3,
    //    Running = 4,
    //    ContinuePending = 5,
    //    PausePending = 6,
    //    Paused = 7
    //}

}
