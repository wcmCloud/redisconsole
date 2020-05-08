using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public class RedisSettingsWindow : Window
    {
        //public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        //public Action OnExit { get; set; }

        public RedisSettingsWindow() : base("Redis Settings", 3)
        {
            InitStyle();
            InitControls();

        }

        public RedisSettingsWindow(string itemKey) : base("Redis Settings", 3)
        {
            InitStyle();
            InitControls(AppProvider.Get(itemKey));
        }

        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(100);
            Height = Dim.Fill();
        }

        public void Close()
        {
            Application.Top.Clear();
            Application.Top?.Remove(this);
        }

        private void InitControls(RedisClient r = null)
        {
            #region nickname
            var nameLabel = new Label(0, 0, "Name");
            var nameText = new TextField(r == null ? "" : r.Name)
            {
                X = Pos.Left(nameLabel),
                Y = Pos.Top(nameLabel) + 1,
                Width = Dim.Fill()
            };
            Add(nameLabel);
            Add(nameText);
            #endregion

            #region host
            var hostLabel = new Label("Host")
            {
                X = Pos.Left(nameText),
                Y = Pos.Top(nameText) + 1
            };
            var hostText = new TextField(r == null ? "" : r.Host)
            {
                X = Pos.Left(hostLabel),
                Y = Pos.Top(hostLabel) + 1,
                Width = Dim.Fill()
            };
            Add(hostLabel);
            Add(hostText);
            #endregion

            #region port
            var portLabel = new Label("Port")
            {
                X = Pos.Left(hostText),
                Y = Pos.Top(hostText) + 1
            };
            var portText = new TextField(r == null ? "6379" : r.Port.ToString())
            {
                X = Pos.Left(portLabel),
                Y = Pos.Top(portLabel) + 1,
                Width = Dim.Fill()
            };
            Add(portLabel);
            Add(portText);
            #endregion

            #region auth
            var authLabel = new Label("Auth")
            {
                X = Pos.Left(portText),
                Y = Pos.Top(portText) + 1
            };
            var authText = new TextField(r == null ? "" : r.Auth)
            {
                X = Pos.Left(authLabel),
                Y = Pos.Top(authLabel) + 1,
                Width = Dim.Fill()
            };
            Add(authLabel);
            Add(authText);
            #endregion

            #region buttons
            var saveButton = new Button("Save", true)
            {
                X = Pos.Left(authText),
                Y = Pos.Top(authText) + 2
            };

            var exitButton = new Button("eXit")
            {
                X = Pos.Right(saveButton) + 5,
                Y = Pos.Top(saveButton)
            };

            Add(exitButton);
            Add(saveButton);
            #endregion

            #region bind-button-events
            saveButton.Clicked = () =>
            {

                if (nameText.Text.ToString().TrimStart().Length == 0)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Name cannot be empty.", "Ok");
                    return;
                }

                if (hostText.Text.ToString().TrimStart().Length == 0)
                {
                    MessageBox.ErrorQuery(25, 8, "Error", "Host cannot be empty.", "Ok");
                    return;
                }

                if (portText.Text.ToString().TrimStart().Length == 0)
                {
                    portText.Text = "6379";
                }

                RedisClient rc = new RedisClient()
                {
                    Name = nameText.Text.ToString(),
                    Host = hostText.Text.ToString(),
                    Port = int.Parse(portText.Text.ToString()),
                    Auth = authText.Text.ToString()
                };
                AppProvider.Store(rc);

                var tframe = Application.Top.Frame;
                var ntop = new Toplevel(tframe);
                var instancesWindow = new RedisInstancesWindow();
                Close();
                ntop.Add(instancesWindow);
                Application.Run(ntop);

                Close();
            };

            exitButton.Clicked = () =>
            {
                var tframe = Application.Top.Frame;
                var ntop = new Toplevel(tframe);
                var instancesWindow = new RedisInstancesWindow();
                Close();
                ntop.Add(instancesWindow);
                Application.Run(ntop);
            };


            #endregion
        }
    }

}

