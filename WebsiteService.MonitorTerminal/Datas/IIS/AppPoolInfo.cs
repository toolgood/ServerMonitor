using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class AppPoolMiniInfo
    {
        public string Name { get; set; }
        public bool Enable32BitAppOnWin64 { get; set; }
        public bool AutoStart { get; set; }
        public StartMode StartMode { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public ManagedPipelineMode ManagedPipelineMode { get; set; }
        public ObjectState State { get; }
        public ProcessModelIdentityType IdentityType { get; set; }
        public string UserName { get; set; }
    }
    public class AppPoolInfo
    {
        public string Name { get; set; }
        public bool Enable32BitAppOnWin64 { get; set; }
        public StartMode StartMode { get; set; }



        public bool AutoStart { get; set; }
        public ApplicationPoolCpu Cpu { get; }
        public ApplicationPoolFailure Failure { get; }
        public ManagedPipelineMode ManagedPipelineMode { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public ApplicationPoolProcessModel ProcessModel { get; }
        public long QueueLength { get; set; }
        public ApplicationPoolRecycling Recycling { get; }
        public ObjectState State { get; }
        public WorkerProcessCollection WorkerProcesses { get; }
    }
}
