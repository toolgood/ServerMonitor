using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{

    public class AppPoolInfo
    {
        public string Name { get; set; }
        public bool Enable32BitAppOnWin64 { get; set; }
        public StartMode StartMode { get; set; }

        public bool AutoStart { get; set; }
        public ApplicationPoolCpu Cpu { get; set; }
        public ApplicationPoolFailure Failure { get; set; }
        public ManagedPipelineMode ManagedPipelineMode { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public ApplicationPoolProcessModel ProcessModel { get; }
        public long QueueLength { get; set; }
        public ApplicationPoolRecycling Recycling { get; }
        public ObjectState State { get; set; }
        public WorkerProcessCollection WorkerProcesses { get; }
    }
    public sealed class ApplicationPoolCpu2
    {
        public ProcessorAction Action { get; set; }
        public long Limit { get; set; }
        public TimeSpan ResetInterval { get; set; }
        public bool SmpAffinitized { get; set; }
        public long SmpProcessorAffinityMask { get; set; }
        public long SmpProcessorAffinityMask2 { get; set; }
    }
    public sealed class ApplicationPoolFailure2  
    {
        public string AutoShutdownExe { get; set; }
        public string AutoShutdownParams { get; set; }
        public LoadBalancerCapabilities LoadBalancerCapabilities { get; set; }
        public string OrphanActionExe { get; set; }
        public string OrphanActionParams { get; set; }
        public bool OrphanWorkerProcess { get; set; }
        public bool RapidFailProtection { get; set; }
        public TimeSpan RapidFailProtectionInterval { get; set; }
        public long RapidFailProtectionMaxCrashes { get; set; }
    }
    public sealed class ApplicationPoolProcessModel2
    {
        public ProcessModelIdentityType IdentityType { get; set; }
        public TimeSpan IdleTimeout { get; set; }
        public IdleTimeoutAction IdleTimeoutAction { get; set; }
        public bool LoadUserProfile { get; set; }
        public long MaxProcesses { get; set; }
        public bool PingingEnabled { get; set; }
        public TimeSpan PingInterval { get; set; }
        public TimeSpan PingResponseTime { get; set; }
        public string Password { get; set; }
        public TimeSpan ShutdownTimeLimit { get; set; }
        public TimeSpan StartupTimeLimit { get; set; }
        public string UserName { get; set; }
        public ProcessModelLogEventOnProcessModel LogEventOnProcessModel { get; set; }
    }
    public sealed class ApplicationPoolRecycling2
    {
        public bool DisallowOverlappingRotation { get; set; }
        public bool DisallowRotationOnConfigChange { get; set; }
        public RecyclingLogEventOnRecycle LogEventOnRecycle { get; set; }
        public ApplicationPoolPeriodicRestart PeriodicRestart { get; }
    }
    public sealed class ApplicationPoolPeriodicRestart2
    {
        public long Memory { get; set; }
        public long PrivateMemory { get; set; }
        public long Requests { get; set; }
        public List<Schedule2> Schedule { get; set; }
        public TimeSpan Time { get; set; }
    }

    public sealed class Schedule2
    {
        public TimeSpan Time { get; set; }
    }

    public sealed class WorkerProcess2  
    {
        public List<ApplicationDomain2> ApplicationDomains { get; set; }
        public string AppPoolName { get; set; }
        public string ProcessGuid { get; set; }
        public int ProcessId { get; set; }
        public WorkerProcessState State { get; set; }

    }
    public class ApplicationDomain2
    {
        public string Id { get; set; }
        public int Idle { get; set; }
        public string PhysicalPath { get; set; }
        public string VirtualPath { get; set; }
    }

}
