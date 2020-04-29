using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebsiteService.MonitorTerminal.Utils;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public class MonitorController : ClientControllerBase
    {
        public MonitorController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        [HttpPost]
        public IActionResult GetMonitorInfo(long timestamp, string sign)
        {
            return View();
        }

        [HttpGet("Monitor/GetProcessInfo")]
        public IActionResult GetProcessInfo(string noticeUrl, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(noticeUrl)] = noticeUrl;
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(500); }
            }

            if (string.IsNullOrEmpty(noticeUrl))
            {
                var pis = ProcessUtil.GetProcessInfos();
                return Json(pis);
            }
            Task.Run(() =>
            {

            });
            return Ok();
        }

        [HttpPost]
        public IActionResult GetAppSetting(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetAppSetting(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BlackupAppSetting(long timestamp, string sign)
        {
            return View();
        }
        [HttpPost]
        public IActionResult RestoreAppSetting(long timestamp, string sign)
        {
            return View();
        }


        [HttpPost]
        public IActionResult RestartMonitor(long timestamp, string sign)
        {
            return View();
        }


        [HttpPost]
        public IActionResult UploadMonitor(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BlackupMonitor(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateMonitor(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestoreMonitor(long timestamp, string sign)
        {
            return View();
        }

    }


}