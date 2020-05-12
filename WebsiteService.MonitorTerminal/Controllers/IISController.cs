using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [HttpGet("IIS/GetSiteList")]
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

        [HttpGet("IIS/GetAppPools")]
        public IActionResult GetAppPools(long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            var r = IISUtil.GetAppPools();
            return Json(r);
        }



        #region 站点 启动 暂停 删除 ResetSite
        [HttpGet("IIS/StartSite")]
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

        [HttpGet("IIS/StopSite")]
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
                IISUtil.StopSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }


        [HttpGet("IIS/ResetSite")]
        public IActionResult ResetSite(string siteName, long timestamp, string sign)
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
                IISUtil.ResetSite(siteName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        [HttpGet("IIS/DeleteSite")]
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

        #region 应用程序池 启动 暂停 删除 回收

        [HttpGet("IIS/StartAppPool")]
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

        [HttpGet("IIS/StopAppPool")]
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
                IISUtil.StopAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        [HttpGet("IIS/DeleteAppPool")]
        public IActionResult DeleteAppPool(string poolName, long timestamp, string sign)
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
                IISUtil.DeleteAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }

        [HttpGet("IIS/RecycleAppPool")]
        public IActionResult RecycleAppPool(string poolName, long timestamp, string sign)
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
                IISUtil.RecycleAppPool(poolName);
                return Ok();
            }
            catch (Exception ex)
            {
            }
            return StatusCode(404);
        }
        #endregion

        [HttpGet("IIS/StartSite")]
        public IActionResult CreateSite(long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }

            return View();
        }

        [HttpGet("IIS/CreateAppPool")]
        public IActionResult CreateAppPool(long timestamp, string sign)
        {
            return View();
        }


        [HttpGet("IIS/UploadWebsite")]
        public IActionResult UploadWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpGet("IIS/BlackupWebsite")]
        public IActionResult BlackupWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpGet("IIS/UpdateWebsite")]
        public IActionResult UpdateWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpGet("IIS/RestoreWebsite")]
        public IActionResult RestoreWebsite(long timestamp, string sign)
        {
            return View();
        }

        [HttpGet("IIS/RestartIIS")]
        public IActionResult RestartIIS(long timestamp, string sign)
        {
            if (IsSignParameter())
            {
                SortedDictionary<string, string> keys = new SortedDictionary<string, string>();
                keys[nameof(timestamp)] = timestamp.ToString();
                if (GetSignHash(keys).Equals(sign, System.StringComparison.CurrentCultureIgnoreCase) == false) { return StatusCode(404); }
            }
            Process.Start("iisreset");
            return Ok();
        }


    }
}