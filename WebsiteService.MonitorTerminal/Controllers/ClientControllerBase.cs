using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
            var str = _configuration.GetValue("AppInfo:SignSalt", "").ToLower().Trim();
            foreach (var item in keys) {
                str += "&";
                str += item.Key;
                str += "=";
                if (string.IsNullOrEmpty(item.Value) == false) {
                    str += item.Value;
                }
            }
            var type = _configuration.GetValue("AppInfo:SignType", "md5").ToLower().Trim();
            switch (type) {
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
            var str = _configuration.GetValue("AppInfo:UseSign", "false").ToLower();
            if (str == "true" || str == "1") {
                return true;
            }
            return false;
        }

        protected async Task SendToNoticeUrl(string noticeUrl, object obj)
        {
            var SignSalt = _configuration.GetValue("AppInfo:SignSalt", "").ToLower().Trim();
            var ServerIp = _configuration.GetValue("AppInfo:ServerIp", "").ToLower().Trim();
            var ServerPort = _configuration.GetValue("AppInfo:ServerPort", "").ToLower().Trim();


            Dictionary<string, string> postData = new Dictionary<string, string>();


            var httpClient = _httpClientFactory.CreateClient();
            var uri = $"http://{ServerIp}:{ServerPort}/{noticeUrl}";
            httpClient.BaseAddress = new Uri(uri);
            var content = new FormUrlEncodedContent(postData);
            await httpClient.PostAsync(uri, content);
        }



 
    }
}
