using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteService.MonitorTerminal.Datas
{
    public class WebsiteInfo
    {
        public string Name { get; set; } = "";

        public string Code { get; set; } = "";

        public string PreReleaseFolder { get; set; } = "";

        public string WebsiteFolder { get; set; } = "";

        public string BackupFolder { get; set; } = "";

        public string UseBackup { get; set; } = "";

        public string BackupRate { get; set; } = "";

        public ExcludeInfo UpdateExclude { get; set; } = new ExcludeInfo();

        public ExcludeInfo BackupExclude { get; set; } = new ExcludeInfo();

    }
    public class ExcludeInfo
    {
        public List<string> FileName { get; set; } = new List<string>();

        public List<string> FolderName { get; set; } = new List<string>();
    }

}
