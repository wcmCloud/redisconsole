using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public static class CUIColorScheme
    {
        public enum ColorSchemeEnum
        {
            Default = 0,
            Dark,
        }


        public static void ApplyTheme(ColorSchemeEnum cs)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (cs)
                {
                    case ColorSchemeEnum.Dark:
                        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
                        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.DarkGray);
                        Colors.Base.HotNormal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
                        Colors.Base.HotFocus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.DarkGray);

                        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
                        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);
                        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.Green, Color.DarkGray);
                        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.Green, Color.Black);
                        Colors.Menu.Disabled = Application.Driver.MakeAttribute(Color.Green, Color.DarkGray);

                        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.Black, Color.DarkGray);
                        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.DarkGray);
                        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.DarkGray);
                        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Green, Color.Black);

                        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.White, Color.Red);
                        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
                        Colors.Error.HotNormal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Red);
                        Colors.Error.HotFocus = Colors.Error.HotNormal;

                        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.Green, Color.DarkGray);
                        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
                        Colors.TopLevel.HotNormal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.DarkGray);
                        Colors.TopLevel.HotFocus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.DarkGray);
                        break;
                    case ColorSchemeEnum.Default:
                    default:
                        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Blue);
                        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);
                        Colors.Base.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Blue);
                        Colors.Base.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Cyan);

                        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Cyan);
                        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);
                        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Cyan);
                        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black);
                        Colors.Menu.Disabled = Application.Driver.MakeAttribute(Color.DarkGray, Color.Cyan);

                        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
                        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);
                        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Gray);
                        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Blue, Color.Cyan);

                        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.White, Color.Red);
                        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
                        Colors.Error.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Red);
                        Colors.Error.HotFocus = Colors.Error.HotNormal;

                        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.Green, Color.Black);
                        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.Cyan);
                        Colors.TopLevel.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black);
                        Colors.TopLevel.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Cyan);

                        break;
                }
                Application.Refresh();
            }

        }
    }
}
