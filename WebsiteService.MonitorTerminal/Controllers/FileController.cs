using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebsiteService.MonitorTerminal.Controllers
{
    public class FileController : ClientControllerBase
    {
        public FileController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}