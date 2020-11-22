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
    public class DataController : BaseController
    {
        private const string typeSeparator = "::";

        public IActionResult Index([FromQuery(Name = "Id")] int id)
        {
            TempData["Id"] = id;
            var inst = AppProvider.Get(id);
            InitView("Showing data for " + inst.Name);

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
        public IActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {
            var id = int.Parse(TempData["Id"].ToString());
            var inst = AppProvider.Get(id);
            RedisStore store = new RedisStore(inst);
            var keys = store.RedisServerKeys();


            List<DataGridViewModel> res = new List<DataGridViewModel>();
            foreach (var k in keys)
                res.Add(new DataGridViewModel(id, k.ToString() , store.GetKeyType(k.ToString()), ""));

            return Json(res.ToDataSourceResult(request));
        }


        #endregion
    }
}