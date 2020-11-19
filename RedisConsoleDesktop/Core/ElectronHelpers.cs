using ElectronNET.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Core
{
    public static class ElectronHelpers
    {
        public static void GoToInstanceIndex()
        {
            GoTo("Instance", "Index");
        }

        public static void GoTo(string controller, string action)
        {
            Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/" + controller + "/" + action);
        }


    }
}
