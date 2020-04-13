using ConsoleUI;
using Microsoft.Extensions.Configuration;
using Redis.Core;
using System;
using System.IO;
using Terminal.Gui;
using ConfigurationProvider = Redis.Core.ConfigurationProvider;

namespace RedisConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var appConfig = config.GetSection("appConfiguration").Get<AppConfiguration>();

            ConfigurationProvider.Configuration = appConfig;

            CUIApplication app = new CUIApplication(appConfig);
            app.InitWindows();
            app.InitMenuBar();
            app.Run();

        }
    }
}
