using Microsoft.AspNetCore.Mvc;

namespace WebsiteServiceClient.Controllers
{
    //Server
    public class ServerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



    }


}