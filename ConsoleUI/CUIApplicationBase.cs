using Redis.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public abstract class CUIApplicationBase
    {

        public FrameView ServerViewFrame { get; set; }
        public Window MainWindow { get; set; }
        public AppConfiguration Configuration { get; set; }


        public CUIApplicationBase(AppConfiguration appConfiguration)
        {
            this.Configuration = appConfiguration;
            Application.Init();
            Application.Current.LayoutStyle = LayoutStyle.Computed;
            CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Dark);
        }

        public void InitMenuBar()
        {
            var top = Application.Top;
            List<MenuBarItem> menuList = new List<MenuBarItem>();
            MenuBarItem mFile = new MenuBarItem("_File", new MenuItem[]{
                        new MenuItem("_Quit", "", Application.RequestStop)
                    });
            menuList.Add(mFile);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                MenuBarItem mTheme = new MenuBarItem("_Theme", new MenuItem[]{
                        new MenuItem("_Relaxed", "", () => CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Default)),
                        new MenuItem("_Dark", "", () => CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Dark)),
                    });
                menuList.Add(mTheme);
            }

            MenuBarItem mHelp = new MenuBarItem("_Help", new MenuItem[]{
                        new MenuItem("_About", "", ()
                                    => MessageBox.Query(50, 8, "About", "Written by " + Configuration.Author + "\n" + Configuration.AssemblyInfoString, "Ok"))
                    });
            menuList.Add(mHelp);


            MenuBar menu = new MenuBar(menuList.ToArray());
            top.Add(menu);
        }

        public void InitWindows()
        {
            MainWindow = new Window(Configuration.AssemblyInfoString)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1,
            };

            Application.Top.Add(MainWindow);

            var instancesWindow = new RedisInstancesWindow(MainWindow);
            MainWindow.Add(instancesWindow);

        }


        public void Run()
        {
            Application.Run();
        }

        private void ApplyTheme()
        {
            ColorScheme c = new ColorScheme();
            c.Normal = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

            //var myColor = Application.Driver.MakeAttribute(Color.Blue, Color.Red);
            //var label = new Label(...);
            //label.TextColor = myColor


        }
    }
}
