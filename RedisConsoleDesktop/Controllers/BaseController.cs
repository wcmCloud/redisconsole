using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using RedisConsoleDesktop.Core;

namespace RedisConsoleDesktop.Controllers
{
    public enum PageTypeEnum
    {
        Index,
        Create,
        Edit,
        Delete,
        Dashboard,
        Other
    }

    public abstract class BaseController : Controller
    {

        public const string BrowseActionName = "Index";
        public const string EditActionName = "Edit";
        public const string CreateActionName = "Create";
        public const string DashboardActionName = "Dashboard";

        public PageTypeEnum PageType
        {
            get; private set;
        }

        [HttpGet, HttpPost]
        public void InitView(string title, string id = null)
        {
            ViewData["ProductName"] = "RedisConsole Desktop";

            ViewData["Title"] = title;
            var buildDt = Assembly.GetEntryAssembly().GetBuildDate();
            ViewData["BuildDate"] = buildDt.ToLongDateString();
            ViewData["BuildTime"] = buildDt.ToShortTimeString() + " UTC";
            ViewData["Version"] = Assembly.GetEntryAssembly().AssemblyInfoString();
            var routeValues = Request.RouteValues;

            var controller = routeValues["controller"].ToString();
            var action = routeValues["action"].ToString();

            PageType = ResolvePageType(action);
            ApplyPageTypeSpecifics(PageType);
            ShowNotifications();
        }

        private void ApplyPageTypeSpecifics(PageTypeEnum pageType)
        {
            ViewBag.PageType = pageType;
            ViewData["PageType"] = pageType;
            switch (pageType)
            {
                case PageTypeEnum.Index:

                    break;
                case PageTypeEnum.Create:
                    ViewData["PrimaryButtonLabel"] = "Create";

                    break;
                case PageTypeEnum.Edit:
                    ViewData["PrimaryButtonLabel"] = "Update";
                    break;
                case PageTypeEnum.Delete:

                    break;
                case PageTypeEnum.Dashboard:
                    ViewData["PrimaryButtonLabel"] = "Dashboard";
                    break;
                case PageTypeEnum.Other:

                    break;

            }
        }

        private PageTypeEnum ResolvePageType(string action)
        {
            PageTypeEnum pageType = PageTypeEnum.Other;
            Enum.TryParse<PageTypeEnum>(action, out pageType);

            return pageType;
        }

        #region notifications

        private void ShowNotifications()
        {
            if (HybridSupport.IsElectronActive)
            {
                Electron.IpcMain.On(Consts.NotificationChannel, (args) =>
                {

                    var listArgs = args.ToString().Split(Consts.MessageDelimiter);
                    MessageBoxOptions mbo = new MessageBoxOptions(listArgs[1]);
                    mbo.Title = "RedisConsoleDesktop";
                    var options = new NotificationOptions(listArgs[0], listArgs[1])
                    {
                        OnClick = async () => await Electron.Dialog.ShowMessageBoxAsync(mbo)
                        // Icon = "/imgs/android-chrome-512x512.png"
                    };
                    Electron.Notification.Show(options);

                });
            }
        }

        #endregion
    }
}
