using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebsiteService.MonitorTerminal.Utils;

namespace WebsiteService.MonitorTerminal.Controllers
{
    //Server
    public class ServerController : ClientControllerBase
    {
        public ServerController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        [HttpGet("Server/GetServiceList")]
        public IActionResult GetServiceList(long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            var r = ServerUtil.GetAllServices();
            return Json(r);
        }

        #region 服务 启动 停止
        [HttpGet("Server/StopService")]
        public IActionResult StopService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(serviceName)] = serviceName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }
            try
            {
                ServerUtil.StopService(serviceName);
                return Ok();
            }
            catch (System.Exception ex)
            {
            }
            return StatusCode(404);
        }

        [HttpGet("Server/StartService")]
        public IActionResult StartService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(serviceName)] = serviceName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }
            try
            {
                ServerUtil.StartService(serviceName);
                return Ok();
            }
            catch (System.Exception ex)
            {
            }
            return StatusCode(404);
        }
        #endregion

        [HttpPost]
        public IActionResult UploadService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }


            return View();
        }

        [HttpPost]
        public IActionResult BlackupService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }

            return View();
        }

        [HttpPost]
        public IActionResult UpdateService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }

            return View();
        }

        [HttpPost]
        public IActionResult RestoreService(string serviceName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (serviceName == "WebsiteService.MonitorTerminal") { return StatusCode(404); }
            if (serviceName == "WebsiteService.AutoGuard") { return StatusCode(404); }

            return View();
        }

    }


}