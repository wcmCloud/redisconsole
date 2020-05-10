using Redis.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public static class MenuProvider
    {

        public static MenuBar GetMenu(AppConfiguration configuration)
        {
            List<MenuBarItem> menuList = new List<MenuBarItem>();
            MenuBarItem mFile = new MenuBarItem("_Console", new MenuItem[]{
                    new MenuItem ("_Quit", "", () => { Exit(); })
                        //new MenuItem("_Quit", "", Application.RequestStop)
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
                                    => MessageBox.Query(50, 8, "About", "Written by " + configuration.Author + "\n" + configuration.AssemblyInfoString, "Ok"))
                    });
            menuList.Add(mHelp);


            return new MenuBar(menuList.ToArray());
        }

        public static void Exit()
        {
            Application.RequestStop();
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
