using ConsoleUI;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Redis.Core;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Terminal.Gui;
using AppProvider = Redis.Core.AppProvider;

namespace RedisConsole
{
    class Program
    {
        //public static IConfigurationRoot configuration;


        static void Main(string[] args)
        {
            try
            {
                if (Debugger.IsAttached)
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                // Load configuration
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

                var config = builder.Build();

                var appConfig = config.GetSection("appConfiguration").Get<AppConfiguration>();
                appConfig.AssemblyInfoString = AssemblyHelpers.AssemblyInfoString(Assembly.GetEntryAssembly());
                AppProvider.Configuration = appConfig;

           

                CUIApplication app = new CUIApplication(appConfig);
                app.InitWindows();
                var top = app.InitMenuBar();

                Logger.Log("application initialised", LogType.Info);

                app.Run(top);

                
            }catch(Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                Logger.Log("application terminated", LogType.Debug);
                Application.RequestStop();
            }

        }

    }
}
