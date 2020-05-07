using System.Collections.Generic;

namespace WebsiteService.MonitorTerminal.Datas.Config
{
    public class EmailInfo
    {
        /// <summary>
        /// 收件人
        /// </summary>
        public List<string> SendEmails { get; set; }

        /// <summary>
        /// 发件箱
        /// </summary>
        public List<Outbox> Outboxes { get; set; } 


        public class Outbox
        {
            public string UserName { get; set; }

            public string Password { get; set; }

            public string Host { get; set; }

            public string Port { get; set; }

            public bool IsSSL { get; set; }
        }

    }



}
