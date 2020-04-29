using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebsiteService.MonitorTerminal.Utils;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public abstract class ClientControllerBase : Controller
    {
        private IConfiguration _configuration;
        private IHttpClientFactory _httpClientFactory;

        public ClientControllerBase(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        protected string GetSignHash(SortedDictionary<string, string> keys)
        {
            var str = _configuration.GetValue("HashSalt", "").ToLower().Trim();
            foreach (var item in keys)
            {
                str += "&";
                str += item.Key;
                str += "=";
                if (string.IsNullOrEmpty(item.Value) == false)
                {
                    str += item.Value;
                }
            }
            var type = _configuration.GetValue("HashType", "md5").ToLower().Trim();
            var h = "";
            switch (type)
            {
                case "crc8": return HashUtil.GetCrc8String(str);
                case "crc16": return HashUtil.GetCrc16String(str);
                case "crc32": return HashUtil.GetCrc32String(str);
                case "md5": return HashUtil.GetMd5String(str);
                case "sha1": return HashUtil.GetSha1String(str);
                case "sha256": return HashUtil.GetSha256String(str);
                case "sha384": return HashUtil.GetSha384String(str);
                case "sha512": return HashUtil.GetSha512String(str);
                default: break;
            }
            return HashUtil.GetMd5String(str);
        }

        protected bool IsSignParameter()
        {
            var str = _configuration.GetValue("HashSign", "false").ToLower();
            if (str == "true" || str == "1")
            {
                return true;
            }
            return false;
        }

        protected void SendToNoticeUrl(string noticeUrl, object obj)
        {
            var str = _configuration.GetValue("HashSalt", "").ToLower().Trim();
            var app = _configuration.GetValue("HashSalt", "").ToLower().Trim();

            var httpClient = _httpClientFactory.CreateClient();
        }


    }
}
