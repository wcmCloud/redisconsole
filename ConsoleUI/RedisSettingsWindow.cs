using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ConsoleUI
{
    public class RedisSettingsWindow : Window
    {
        private readonly View _parent;
        public Action<(string name, DateTime birthday)> OnLogin { get; set; }
        public Action OnExit { get; set; }

        public RedisSettingsWindow(View parent) : base("Redis Settings", 3)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }

        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(60);
            Height = 18;
        }

        public void Close()
        {
            _parent?.Remove(this);
        }

        private void InitControls()
        {
            #region nickname
            var nameLabel = new Label(0, 0, "Name");
            var nameText = new TextField("")
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
            var hostText = new TextField("")
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
            var portText = new TextField("")
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
            var authText = new TextField("")
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

            var exitButton = new Button("Exit")
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

                //var isDateValid = DateTime.TryParse(birthText.Text.ToString(), out DateTime birthDate);

                //if (string.IsNullOrEmpty(birthText.Text.ToString()) || !isDateValid)
                //{
                //    MessageBox.ErrorQuery(25, 8, "Error", "Date is required\nor is invalid.", "Ok");
                //    return;
                //}

                //OnLogin?.Invoke((name: nameText.Text.ToString(), birthday: birthDate));

                Close();
            };

            exitButton.Clicked = () =>
            {
                OnExit?.Invoke();
                Close();
            };
            #endregion
        }
    }

}

