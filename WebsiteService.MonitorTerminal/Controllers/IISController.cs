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
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            var r = IISUtil.GetSites();
            return Json(r);
        }

        #region 站点 启动 暂停 删除
        public IActionResult StartSite(string siteName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(siteName)] = siteName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.StartSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        public IActionResult StopSite(string siteName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(siteName)] = siteName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.StartSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        public IActionResult DeleteSite(string siteName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(siteName)] = siteName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.DeleteSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        #endregion

        #region 应用程序池 启动 暂停 删除

        public IActionResult StartAppPool(string poolName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(poolName)] = poolName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.StartAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        public IActionResult StopAppPool(string poolName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(poolName)] = poolName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.StartAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        public IActionResult DeleteAppPool(string poolName, long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(poolName)] = poolName.ToString();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            try
            {
                IISUtil.DeleteAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        #endregion


        public IActionResult CreateSite(long timestamp, string sign)
        {
            return View();
        }

        public IActionResult CreateAppPool(long timestamp, string sign)
        {
            return View();
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