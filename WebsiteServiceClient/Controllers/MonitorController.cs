using Microsoft.AspNetCore.Mvc;

namespace WebsiteServiceClient.Controllers
{
    public class MonitorController : ClientControllerBase
    {
        [HttpPost]
        public IActionResult GetMonitorInfo(long timestamp,string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetProcessInfo(long timestamp,string sign)
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAppSetting(long timestamp,string sign){
            return View();
        }

        [HttpPost]
        public IActionResult SetAppSetting(long timestamp,string sign){
            return View();
        }

        [HttpPost]
        public IActionResult BlackupAppSetting(long timestamp,string sign){
            return View();
        }
        [HttpPost]
        public IActionResult RestoreAppSetting(long timestamp,string sign){
            return View();
        }


        [HttpPost]
        public IActionResult RestartMonitor(long timestamp,string sign){
            return View();
        }


        [HttpPost]
        public IActionResult UploadMonitor(long timestamp,string sign){
            return View();
        }

        [HttpPost]
        public IActionResult BlackupMonitor(long timestamp,string sign){
            return View();
        }

        [HttpPost]
        public IActionResult UpdateMonitor(long timestamp,string sign){
            return View();
        }

        [HttpPost]
        public IActionResult RestoreMonitor(long timestamp,string sign)
        {
            return View();
        }

    }


}