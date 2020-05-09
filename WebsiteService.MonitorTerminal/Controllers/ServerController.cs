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

        [HttpPost]
        public IActionResult GetServiceList(long timestamp, string sign)
        {
            var r = ServerUtil.GetAllServices();
            return Json(r);
        }

        [HttpPost]
        public IActionResult StopService(string serviceName, long timestamp, string sign)
        {
            try
            {
                ServerUtil.StopService(serviceName);
                return Ok();
            }
            catch (System.Exception ex)
            {
            }
            return StatusCode(500);
        }

        [HttpPost]
        public IActionResult StartService(string serviceName, long timestamp, string sign)
        {
            try
            {
                ServerUtil.StartService(serviceName);
                return Ok();
            }
            catch (System.Exception ex)
            {
            }
            return StatusCode(500);
        }

        [HttpPost]
        public IActionResult UploadService(string serviceName, long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BlackupService(string serviceName, long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateService(string serviceName, long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestoreService(string serviceName, long timestamp, string sign)
        {
            return View();
        }

    }


}