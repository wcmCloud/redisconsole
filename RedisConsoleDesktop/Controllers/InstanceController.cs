using ElectronNET.API;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Core;
using RedisConsoleDesktop.Core;
using RedisConsoleDesktop.ModelBinders;
using RedisConsoleDesktop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var keys = AppProvider.GetKeys();
            List<InstanceGridViewModel> res = new List<InstanceGridViewModel>();
            foreach (var k in keys)
                res.Add(new InstanceGridViewModel(k));

            return Json(res.ToDataSourceResult(request));
        }


        [HttpPost]
        public IActionResult Create(IFormCollection data, [ModelBinder(BinderType = typeof(InstanceSettingsModelBinder))] InstanceSettingsViewModel redisInstance)
        {

            RedisClient rc = new RedisClient()
            {
                Name = redisInstance.Name,
                Host = redisInstance.Host,
                Port = redisInstance.Port,
                Auth = redisInstance.Auth
            };
            AppProvider.Store(rc);


            if (HybridSupport.IsElectronActive)
            {
                ElectronHelpers.GoToInstanceIndex();
            }
            else
            {
                return Content(Url.Action("Index", "Instace"));
            }
          
            return View();
        }


    }
}
