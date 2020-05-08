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

        public Toplevel InitMenuBar()
        {
            var top = Application.Top;
            top.Add(MenuProvider.GetMenu(Configuration));
            return top;
        }

        public void InitWindows()
        {
            MainWindow = new RedisInstancesWindow();
            Application.Top.Add(MainWindow);
        }


        public void Run(Toplevel top)
        {
            Application.Run(top);
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
