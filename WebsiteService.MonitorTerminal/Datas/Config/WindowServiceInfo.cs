using System.Collections.Generic;

namespace WebsiteService.MonitorTerminal.Datas.Config
{
    public class WindowServiceInfo
    {
        /// <summary>
        /// 预发布文件夹 - 总的
        /// </summary>
        public string PreReleaseFolder { get; set; }

        /// <summary>
        /// 发布文件夹 - 总的
        /// </summary>
        public string ReleaseFolder { get; set; }

        /// <summary>
        /// 备份文件夹 - 总的
        /// </summary>
        public string BackupFolder { get; set; }

        /// <summary>
        /// 日志文件夹 - 总的
        /// </summary>
        public string LogFolder { get; set; }


        public ExcludeInfo Exclude { get; set; }


        public List<WindowServiceItemInfo> Items { get; set; }

    }

    public class WindowServiceItemInfo
    {
        public string Name { get; set; }

        /// <summary>
        /// 是否保持运行
        /// </summary>
        public bool IsKeeping { get; set; }


        public ExcludeInfo Exclude { get; set; }


        /// <summary>
        /// 预发布文件夹  
        /// </summary>
        public string PreReleaseFolder { get; set; }

        /// <summary>
        /// 发布文件夹 
        /// </summary>
        public string ReleaseFolder { get; set; }

        /// <summary>
        /// 备份文件夹 
        /// </summary>
        public string BackupFolder { get; set; }

        /// <summary>
        /// 日志文件夹 
        /// </summary>
        public string LogFolder { get; set; }
    }

}
