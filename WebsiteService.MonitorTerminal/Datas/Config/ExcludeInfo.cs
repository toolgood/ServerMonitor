using System.Collections.Generic;

namespace WebsiteService.MonitorTerminal.Datas.Config
{
    public class ExcludeInfo
    {
        public List<string> FileName { get; set; } = new List<string>();


        public List<string> FolderName { get; set; } = new List<string>();


        public List<string> Paths { get; set; } = new List<string>();
    }

}
