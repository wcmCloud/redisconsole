using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Redis.Core;
using RedisConsoleDesktop.Models;

namespace RedisConsoleDesktop.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            
            InitView(AppProvider.Configuration.Author);
            return View();
        }







    }
}
