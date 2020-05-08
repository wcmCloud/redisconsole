using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public abstract class ConsoleWindowBase : Window
    {
        public ConsoleWindowBase(string title) : base(title + " - " + AppProvider.Configuration.AssemblyInfoString, 1)
        {


        }
    }
}
