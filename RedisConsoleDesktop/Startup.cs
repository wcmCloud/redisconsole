using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Runtime.InteropServices;
using System.IO;
using Redis.Core;
using System.Reflection;
using RedisConsoleDesktop.Core;

namespace RedisConsoleDesktop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                // Maintain property names during serialization. See:
                // https://github.com/aspnet/Announcements/issues/194
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());


            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(240);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add Kendo UI services to the services container
            services.AddKendo();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Instance}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var appConfig = config.GetSection("appConfiguration").Get<AppConfiguration>();
            appConfig.AssemblyInfoString = Core.AssemblyHelpers.AssemblyInfoString(Assembly.GetEntryAssembly());
            AppProvider.Configuration = appConfig;


            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }
        }

        private async void ElectronBootstrap()
        {
            CreateMenu();
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1152,
                Height = 940,
                Icon = "favicon.ico",
                Show = false
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            //browserWindow.SetTitle("Electron.NET API Demos"); ;
            browserWindow.OnClosed += () =>
            {
                Electron.App.Quit();
            };
        }

        private void CreateMenu()
        {
            bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            MenuItem[] menu = null;

            MenuItem[] aboutMenu = new MenuItem[]
            {
             new MenuItem
                    {
                        Label = "About this app",
                        //Role = MenuRole.about,
                        Click = async () => {
                            var options = new MessageBoxOptions("Copyright © 2021 Redis Console - a cross platform Redis database management console - All Rights Reserved.");
                            options.Type = MessageBoxType.info;
                            options.Title = "RedisConsole Deksktop";
                            await Electron.Dialog.ShowMessageBoxAsync(options);
                        }
                    },
            new MenuItem { Type = MenuType.separator },
            new MenuItem
                    {
                        Label = "Learn More",
                        Click = async () => await Electron.Shell.OpenExternalAsync("https://redisconsole.com")
                    },
            new MenuItem { Type = MenuType.separator },
            new MenuItem { Role = MenuRole.toggledevtools },
            };

            MenuItem[] fileMenu = new MenuItem[]
            {
        new MenuItem { Role = isMac ? MenuRole.close : MenuRole.quit }
            };


            MenuItem[] editMenu = new MenuItem[]
         {
        new MenuItem { Role = MenuRole.cut },
        new MenuItem { Role = MenuRole.copy},
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.paste },

         };

            MenuItem[] viewMenu = new MenuItem[]
           {
        new MenuItem { Role = MenuRole.reload },
        new MenuItem { Role = MenuRole.forcereload },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.resetzoom },
        new MenuItem { Role = MenuRole.zoomin },
        new MenuItem { Role = MenuRole.zoomout },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.togglefullscreen },
        new MenuItem { Type = MenuType.separator },
           };

            Action c = new Action(ElectronHelpers.GoToInstanceIndex);
            MenuItem[] redisMenu = new MenuItem[]
            {
            new MenuItem { Label = "Servers", Type = MenuType.normal, Click = c}
            };

            if (isMac)
            {
                menu = new MenuItem[]
                {
            new MenuItem { Label = "Electron", Type = MenuType.submenu, Submenu = aboutMenu  },
            new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },
            new MenuItem { Label = "Edit", Type = MenuType.submenu, Submenu = editMenu },
            new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu},
            new MenuItem { Label = "Redis", Type = MenuType.submenu, Submenu = redisMenu  },
            new MenuItem { Label = "Help", Type = MenuType.submenu, Submenu = aboutMenu }
                };
            }
            else
            {
                menu = new MenuItem[]
                {
            new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },
            new MenuItem { Label = "Edit", Type = MenuType.submenu, Submenu = editMenu },
            new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu},
            new MenuItem { Label = "Redis", Type = MenuType.submenu, Submenu = redisMenu  },
            new MenuItem { Label = "Help", Type = MenuType.submenu, Submenu = aboutMenu }
                };
            }

            Electron.Menu.SetApplicationMenu(menu);
        }

      

    }
}
