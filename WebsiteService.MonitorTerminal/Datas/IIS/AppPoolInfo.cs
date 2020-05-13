using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{

    public class AppPoolInfo
    {
        public AppPoolInfo() { }
        public AppPoolInfo(ApplicationPool pool)
        {
            Name = pool.Name;
            Enable32BitAppOnWin64 = pool.Enable32BitAppOnWin64;
            StartMode = pool.StartMode;
            AutoStart = pool.AutoStart;
            ManagedPipelineMode = pool.ManagedPipelineMode;
            ManagedRuntimeVersion = pool.ManagedRuntimeVersion;
            QueueLength = pool.QueueLength;
            State = pool.State;
            Cpu = new ApplicationPoolCpu2(pool.Cpu);
            Failure = new ApplicationPoolFailure2(pool.Failure);
            ProcessModel = new ApplicationPoolProcessModel2(pool.ProcessModel);
            Recycling = new ApplicationPoolRecycling2(pool.Recycling);
            WorkerProcesses = new List<WorkerProcess2>();
            foreach (var item in pool.WorkerProcesses)
            {
                WorkerProcesses.Add(new WorkerProcess2(item));
            }
        }
        public string Name { get; set; }
        public bool Enable32BitAppOnWin64 { get; set; }
        public StartMode StartMode { get; set; }
        public bool AutoStart { get; set; }
        public ManagedPipelineMode ManagedPipelineMode { get; set; }
        public string ManagedRuntimeVersion { get; set; }
        public long QueueLength { get; set; }
        public ObjectState State { get; set; }

        public ApplicationPoolCpu2 Cpu { get; set; }
        public ApplicationPoolFailure2 Failure { get; set; }
        public ApplicationPoolProcessModel2 ProcessModel { get; }
        public ApplicationPoolRecycling2 Recycling { get; }
        public List<WorkerProcess2> WorkerProcesses { get; }
    }
    public sealed class ApplicationPoolCpu2
    {
        public ApplicationPoolCpu2() { }
        public ApplicationPoolCpu2(ApplicationPoolCpu cpu)
        {
            Action = cpu.Action;
            Limit = cpu.Limit;
            ResetInterval = cpu.ResetInterval;
            SmpAffinitized = cpu.SmpAffinitized;
            SmpProcessorAffinityMask = cpu.SmpProcessorAffinityMask;
            SmpProcessorAffinityMask2 = cpu.SmpProcessorAffinityMask2;
        }
        public ProcessorAction Action { get; set; }
        public long Limit { get; set; }
        public TimeSpan ResetInterval { get; set; }
        public bool SmpAffinitized { get; set; }
        public long SmpProcessorAffinityMask { get; set; }
        public long SmpProcessorAffinityMask2 { get; set; }
    }
    public sealed class ApplicationPoolFailure2
    {
        public ApplicationPoolFailure2() { }
        public ApplicationPoolFailure2(ApplicationPoolFailure failure)
        {
            AutoShutdownExe = failure.AutoShutdownExe;
            AutoShutdownParams = failure.AutoShutdownParams;
            LoadBalancerCapabilities = failure.LoadBalancerCapabilities;
            OrphanActionExe = failure.OrphanActionExe;
            OrphanActionParams = failure.OrphanActionParams;
            OrphanWorkerProcess = failure.OrphanWorkerProcess;
            RapidFailProtection = failure.RapidFailProtection;
            RapidFailProtectionInterval = failure.RapidFailProtectionInterval;
            RapidFailProtectionMaxCrashes = failure.RapidFailProtectionMaxCrashes;
        }

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
        public ApplicationPoolProcessModel2() { }
        public ApplicationPoolProcessModel2(ApplicationPoolProcessModel model)
        {
            IdentityType = model.IdentityType;
            IdleTimeout = model.IdleTimeout;
            IdleTimeoutAction = model.IdleTimeoutAction;
            LoadUserProfile = model.LoadUserProfile;
            MaxProcesses = model.MaxProcesses;
            PingingEnabled = model.PingingEnabled;
            PingInterval = model.PingInterval;
            PingResponseTime = model.PingResponseTime;
            Password = model.Password;
            ShutdownTimeLimit = model.ShutdownTimeLimit;
            StartupTimeLimit = model.StartupTimeLimit;
            UserName = model.UserName;
            LogEventOnProcessModel = model.LogEventOnProcessModel;
        }

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
        public ApplicationPoolRecycling2() { }
        public ApplicationPoolRecycling2(ApplicationPoolRecycling recycling)
        {
            DisallowOverlappingRotation = recycling.DisallowOverlappingRotation;
            DisallowRotationOnConfigChange = recycling.DisallowRotationOnConfigChange;
            LogEventOnRecycle = recycling.LogEventOnRecycle;
            PeriodicRestart = new ApplicationPoolPeriodicRestart2(recycling.PeriodicRestart);
        }

        public bool DisallowOverlappingRotation { get; set; }
        public bool DisallowRotationOnConfigChange { get; set; }
        public RecyclingLogEventOnRecycle LogEventOnRecycle { get; set; }
        public ApplicationPoolPeriodicRestart2 PeriodicRestart { get; }
    }
    public class ApplicationPoolPeriodicRestart2
    {
        public ApplicationPoolPeriodicRestart2() { }
        public ApplicationPoolPeriodicRestart2(ApplicationPoolPeriodicRestart restart)
        {
            Memory = restart.Memory;
            PrivateMemory = restart.PrivateMemory;
            Requests = restart.Requests;
            Time = restart.Time;
            Schedule = new List<Schedule2>();
            foreach (var item in restart.Schedule)
            {
                Schedule.Add(new Schedule2(item));
            }
        }
        public long Memory { get; set; }
        public long PrivateMemory { get; set; }
        public long Requests { get; set; }
        public List<Schedule2> Schedule { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class Schedule2
    {
        public Schedule2() { }
        public Schedule2(Schedule schedule)
        {
            Time = schedule.Time;
        }
        public TimeSpan Time { get; set; }
    }

    public class WorkerProcess2
    {
        public WorkerProcess2() { }
        public WorkerProcess2(WorkerProcess process)
        {
            AppPoolName = process.AppPoolName;
            ProcessGuid = process.ProcessGuid;
            ProcessId = process.ProcessId;
            State = process.State;
            ApplicationDomains = new List<ApplicationDomain2>();
            foreach (var item in process.ApplicationDomains)
            {
                ApplicationDomains.Add(new ApplicationDomain2(item));
            }
        }

        public List<ApplicationDomain2> ApplicationDomains { get; set; }
        public string AppPoolName { get; set; }
        public string ProcessGuid { get; set; }
        public int ProcessId { get; set; }
        public WorkerProcessState State { get; set; }

    }
    public class ApplicationDomain2
    {
        public ApplicationDomain2() { }
        public ApplicationDomain2(ApplicationDomain domain)
        {
            Id = domain.Id;
            Idle = domain.Idle;
            PhysicalPath = domain.PhysicalPath;
            VirtualPath = domain.VirtualPath;
        }

        public string Id { get; set; }
        public int Idle { get; set; }
        public string PhysicalPath { get; set; }
        public string VirtualPath { get; set; }
    }

}
