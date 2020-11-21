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
using System.Linq;
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

        #region Create
        public IActionResult Create()
        {

            InitView("Connect a new instance");
            var model = new InstanceSettingsViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(IFormCollection data, [ModelBinder(BinderType = typeof(InstanceSettingsModelBinder))] InstanceSettingsViewModel redisInstance)
        {

            RedisClient rc = new RedisClient()
            {
                Id = redisInstance.Id,
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

        #endregion

        #region Edit

        [HttpGet]
        public IActionResult Edit([FromQuery(Name = "Id")] int id)
        {
            var inst = AppProvider.Get(id);
            InitView("Edit " + inst.Name);
            var model = new InstanceSettingsViewModel(inst);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection data, [ModelBinder(BinderType = typeof(InstanceSettingsModelBinder))] InstanceSettingsViewModel redisInstance)
        {
            var inst = AppProvider.Get(redisInstance.Id);
            InitView("Edit " + redisInstance.Name);

            inst.Name = redisInstance.Name;
            inst.Host = redisInstance.Host;
            inst.Port = redisInstance.Port;
            inst.Auth = redisInstance.Auth;


            AppProvider.Store(inst);


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
        #endregion

        #region Grid
        public IActionResult Instances_Read([DataSourceRequest] DataSourceRequest request)
        {
            var keys = AppProvider.GetKeys();
            List<InstanceGridViewModel> res = new List<InstanceGridViewModel>();
            foreach (var k in keys)
                res.Add(new InstanceGridViewModel(k.Item1, k.Item2));

            return Json(res.ToDataSourceResult(request));
        }

        [HttpPost]
        [HttpGet]
        public IActionResult InstanceInfo_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var inst = AppProvider.Get(id);
            RedisStore store = new RedisStore(inst);
            var rediskeys = store.GenerateServerInfoDictionary().Select(p => new InstanceInfoGridViewModel(p));


            return Json(rediskeys.ToDataSourceResult(request));
        }

        #endregion
    }
}