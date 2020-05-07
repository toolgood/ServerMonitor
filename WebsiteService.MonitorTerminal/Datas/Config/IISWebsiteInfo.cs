using System.Collections.Generic;

namespace WebsiteService.MonitorTerminal.Datas.Config
{
    public class IISWebsiteInfo
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

        /// <summary>
        /// 发布排除文件
        /// </summary>
        public ExcludeInfo ReleaseExclude { get; set; }

        /// <summary>
        /// 备份排除
        /// </summary>
        public ExcludeInfo BackupExclude { get; set; }


        public List<WindowServiceItemInfo> Items { get; set; }

    }
    public class IISWebsiteItemInfo
    {
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseLetsEncrypt { get; set; }

        /// <summary>
        /// 是否保持运行
        /// </summary>
        public bool IsKeeping { get; set; }

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
    }





}
