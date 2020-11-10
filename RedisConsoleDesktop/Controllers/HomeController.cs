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



        public IActionResult Instances_Read([DataSourceRequest] DataSourceRequest request)
        {
            var keys = AppProvider.GetKeys(); ;
            List<RedisInstanceGridViewModel> res = new List<RedisInstanceGridViewModel>();
            foreach (var k in keys)
                res.Add(new RedisInstanceGridViewModel(k));

            res.Add(new RedisInstanceGridViewModel("Redis instance 1"));
            res.Add(new RedisInstanceGridViewModel("Redis instance 2"));
            res.Add(new RedisInstanceGridViewModel("Redis instance 3"));
            res.Add(new RedisInstanceGridViewModel("Redis instance 4"));
            return Json(res.ToDataSourceResult(request));
        }





    }
}
