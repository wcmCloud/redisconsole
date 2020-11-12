using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Redis.Core;
using RedisConsoleDesktop.Models;

namespace RedisConsoleDesktop.Controllers
{
    public class InstanceController : BaseController
    {
        public IActionResult Index()
        {
            
            InitView("Redis Instances");
            return View();
        }

        public IActionResult Create()
        {

            InitView("Connect a new instance");
            //return View();
            List<InstanceSettingsViewModel> m = new List<InstanceSettingsViewModel>();
            m.Add(new InstanceSettingsViewModel());
            var model = new InstanceSettingsViewModel();

            return View(model);
        }

        public IActionResult Edit()
        {

            InitView("Edit");
            return View();
        }


        public IActionResult Instances_Read([DataSourceRequest] DataSourceRequest request)
        {
            var keys = AppProvider.GetKeys(); ;
            List<InstanceGridViewModel> res = new List<InstanceGridViewModel>();
            foreach (var k in keys)
                res.Add(new InstanceGridViewModel(k));

            res.Add(new InstanceGridViewModel("Redis instance X1"));
            res.Add(new InstanceGridViewModel("Redis instance X2"));
            res.Add(new InstanceGridViewModel("Redis instance X3"));
            res.Add(new InstanceGridViewModel("Redis instance X4"));
            return Json(res.ToDataSourceResult(request));
        }





    }
}
