using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebsiteService.MonitorTerminal.Utils;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public class IISController : ClientControllerBase
    {
        public IISController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        public IActionResult GetSiteList(long timestamp, string sign)
        {
            var r = IISUtil.GetSites();
            return Json(r);
        }

        public IActionResult StartSite(string siteName, long timestamp, string sign)
        {
            try
            {
                IISUtil.StartSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }

        public IActionResult StopSite(string siteName, long timestamp, string sign)
        {
            try
            {
                IISUtil.StartSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }

        public IActionResult CreateSite(long timestamp, string sign)
        {
            return View();
        }

        public IActionResult DeleteSite(string siteName, long timestamp, string sign)
        {
            try
            {
                IISUtil.DeleteSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }


        public IActionResult StartAppPool(string poolName, long timestamp, string sign)
        {
            try
            {
                IISUtil.StartAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }

        public IActionResult StopAppPool(string poolName, long timestamp, string sign)
        {
            try
            {
                IISUtil.StartAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }

        public IActionResult CreateAppPool(long timestamp, string sign)
        {
            return View();
        }

        public IActionResult DeleteAppPool(string poolName, long timestamp, string sign)
        {
            try
            {
                IISUtil.DeleteAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(500);
        }


        [HttpPost]
        public IActionResult UploadWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BlackupWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestoreWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestartIIS(long timestamp, string sign)
        {
            return View();
        }


    }
}