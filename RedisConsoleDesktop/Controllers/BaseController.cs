using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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


    }
}
