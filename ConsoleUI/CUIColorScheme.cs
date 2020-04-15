using System;
using System.Collections.Generic;
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
            BlackWhite
        }


        public static void ApplyTheme(ColorSchemeEnum cs)
        {

            //switch (cs)
            //{
            //    case ColorSchemeEnum.Dark:
            //        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
            //        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Green);
            //        Colors.Base.HotFocus = Application.Driver.MakeAttribute(Color.Red, Color.BrightGreen);
            //        Colors.Base.HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Green);
            //        Colors.Base.Disabled = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
            //        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Green);
            //        Colors.TopLevel.HotFocus = Application.Driver.MakeAttribute(Color.Red, Color.BrightGreen);
            //        Colors.TopLevel.HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Green);
            //        Colors.TopLevel.Disabled = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
            //        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);
            //        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
            //        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
            //        Colors.Menu.Disabled = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.Gray, Color.BrightGreen);
            //        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow);
            //        Colors.Dialog.Disabled = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
            //        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Error.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Error.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow);
            //        Colors.Error.Disabled = Application.Driver.MakeAttribute(Color.Blue, Color.Green);
            //        break;
            //    case ColorSchemeEnum.BlackWhite:
            //        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
            //        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.DarkGray, Color.White);
            //        Colors.Base.HotFocus = Application.Driver.MakeAttribute(Color.White, Color.Gray);
            //        Colors.Base.HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
            //        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);
            //        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.Gray, Color.Black);
            //        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.Gray, Color.DarkGray);

            //        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
            //        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
            //        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.White);
            //        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.White);

            //        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
            //        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Error.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrightGreen);
            //        Colors.Error.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow);
            //        break;
            //    case ColorSchemeEnum.Default:
            //    default:
            //        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
            //        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);
            //        Colors.Base.HotFocus = Application.Driver.MakeAttribute(Color.Red, Color.BrightGreen);
            //        Colors.Base.HotNormal = Application.Driver.MakeAttribute(Color.Blue, Color.Green);

            //        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
            //        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.Black);
            //        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Black);
            //        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.DarkGray);

            //        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.Cyan, Color.BrightYellow);
            //        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.DarkGray, Color.Cyan);
            //        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.Cyan);
            //        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow);

            //        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.DarkGray);
            //        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.DarkGray, Color.Cyan);
            //        Colors.Error.HotFocus = Application.Driver.MakeAttribute(Color.Black, Color.BrighCyan);
            //        Colors.Error.HotNormal = Application.Driver.MakeAttribute(Color.Black, Color.BrightYellow);

            //        break;
            //}


        }
    }
}
