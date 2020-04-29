﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public class IISController : ClientControllerBase
    {
        public IISController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        public IActionResult GetSiteList(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult StartSite(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult StopSite(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult CreateSite(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult DeleteSite(long timestamp,string sign)
        {
            return View();
        }


        public IActionResult StartAppPool(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult StopAppPool(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult CreateAppPool(long timestamp,string sign)
        {
            return View();
        }

        public IActionResult DeleteAppPool(long timestamp,string sign)
        {
            return View();
        }


        [HttpPost]
        public IActionResult UploadWebsite(long timestamp,string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BlackupWebsite(long timestamp,string sign)
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult UpdateWebsite(long timestamp,string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestoreWebsite(long timestamp,string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RestartIIS(long timestamp,string sign)
        {
            return View();
        }


    }
}