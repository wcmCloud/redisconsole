using Redis.Core;
using System;
using System.Collections.Generic;
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
            CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Default);
        }

        public void InitMenuBar()
        {
            var top = Application.Top;

            top.Add(
                 new MenuBar(new MenuBarItem[] {
                    new MenuBarItem("_File", new MenuItem[]{
                        new MenuItem("_Quit", "", Application.RequestStop)
                    }), // end of file menu
                    new MenuBarItem("_Servers", new MenuItem[]{
                        new MenuItem("_Add Redis Server", "", () =>{
                        // settings window will be appear on the center screen
                        var settingsWindow = new RedisSettingsWindow(MainWindow);
                        MainWindow.Add(settingsWindow);
                        })
                    }), // end of Servers menu
                    new MenuBarItem("_Theme", new MenuItem[]{
                        new MenuItem("_Relaxed", "", () => CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Default)),
                        new MenuItem("_Dark", "", () => CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.Dark)),
                        new MenuItem("_Black & White", "", () => CUIColorScheme.ApplyTheme(CUIColorScheme.ColorSchemeEnum.BlackWhite)),
                    }), // end of Servers menu
                    new MenuBarItem("_Help", new MenuItem[]{
                        new MenuItem("_About", "", ()
                                    => MessageBox.Query(50, 5, "About", "Written by Christos Christodoulidis\nVersion: 0.0001", "Ok"))
                    }) // end of the help menu
                 })
             );


        }

        public void InitWindows()
        {
            MainWindow = new Window(Configuration.Name)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1,
            };

            Application.Top.Add(MainWindow);


            #region server-view
            ServerViewFrame = new FrameView("Servers")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(25),
                Height = Dim.Percent(80),
            };

            var serverView = new ListView
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            
            
            //serverView.Add(new Label())
            
                ServerViewFrame.Add(serverView);
            MainWindow.Add(ServerViewFrame);
            #endregion

            #region server-view
            var instanceViewFrame = new FrameView("Instance")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(25),
                Height = Dim.Percent(80),
            };


            MainWindow.Add(instanceViewFrame);
            #endregion

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
