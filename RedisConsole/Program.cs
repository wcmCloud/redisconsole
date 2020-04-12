using ConsoleUI;
using System;
using Terminal.Gui;

namespace RedisConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CUIApplication app = new CUIApplication();
            app.InitMenuBar();
            app.InitWindows();
            app.Run();

        }
    }
}
