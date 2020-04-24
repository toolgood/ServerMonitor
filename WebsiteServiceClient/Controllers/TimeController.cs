using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteServiceClient.Utils;

namespace WebsiteServiceClient.Controllers
{
    public class TimeController : ClientControllerBase
    {
        [HttpPost]
        public IActionResult GetTime(long timestamp, string sign)
        {

            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpPost]
        public IActionResult SetTime(DateTime newTime, long timestamp, string sign)
        {
            TimeUtil.SetDate(newTime);
            return Ok();
        }




    }


}