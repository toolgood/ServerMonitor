using Microsoft.Web.Administration;

namespace WebsiteService.MonitorTerminal.Datas.IIS
{
    public class AppPoolMiniInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 启用32位
        /// </summary>
        public bool Enable32BitAppOnWin64 { get; set; }
        /// <summary>
        /// 自动启动
        /// </summary>
        public bool AutoStart { get; set; }
        /// <summary>
        /// 启动模式 0）按需，1）一直运行 
        /// </summary>
        public StartMode StartMode { get; set; }
        /// <summary>
        /// .net CLR 版本，v2.0 v4.0 空， 空为无托管代码
        /// </summary>
        public string ManagedRuntimeVersion { get; set; }
        /// <summary>
        /// 模式，0）集成   1）经典
        /// </summary>
        public ManagedPipelineMode ManagedPipelineMode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public ObjectState State { get; }
        /// <summary>
        /// 标识
        /// </summary>
        public ProcessModelIdentityType IdentityType { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 网站数
        /// </summary>
        public int SiteCount { get; set; }
    }
}
