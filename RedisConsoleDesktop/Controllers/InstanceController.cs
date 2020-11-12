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

            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On("basic-noti", (args) => {

                    var options = new NotificationOptions("Basic Notification", "Short message part")
                    {
                        OnClick = async () => await Electron.Dialog.ShowMessageBoxAsync("Notification clicked")
                    };

                    Electron.Notification.Show(options);

                });

                Electron.IpcMain.On("advanced-noti", (args) => {

                    var options = new NotificationOptions("Notification with image", "Short message plus a custom image")
                    {
                        OnClick = async () => await Electron.Dialog.ShowMessageBoxAsync("Notification clicked"),
                        Icon = "/imgs/android-chrome-512x512.png"
                    };

                    Electron.Notification.Show(options);
                });
            }

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
