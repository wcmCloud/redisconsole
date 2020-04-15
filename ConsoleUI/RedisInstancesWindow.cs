using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
namespace ConsoleUI
{
    public class RedisInstancesWindow : Window
    {

        private readonly View _parent;
        //public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        //public Action OnExit { get; set; }
        private List<string> keys;

        public RedisInstancesWindow(View parent) : base("Redis Instances", 3)
        {
            _parent = parent;
            InitControls();
            InitStyle();
        }
        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(80);
            Height = 22;
        }

        public void Close()
        {
            _parent?.Remove(this);
        }

        private void InitControls()
        {
            keys = Redis.Core.AppProvider.GetKeys();
            ListView lv = new ListView(keys)
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(100),
                Height = 12
            };
            Add(lv);




            #region buttons
            var connectButton = new Button("Connect", true)
            {
                X = Pos.Left(lv),
                Y = Pos.Bottom(lv) + 1
            };

            var editButton = new Button("Edit")
            {
                X = Pos.Right(connectButton) + 5,
                Y = Pos.Top(connectButton)
            };

            var newButton = new Button("New")
            {
                X = Pos.Right(editButton) + 5,
                Y = Pos.Top(editButton)
            };

            var deleteButton = new Button("Delete")
            {
                X = Pos.Right(newButton) + 5,
                Y = Pos.Top(newButton)
            };

            Add(editButton);
            Add(connectButton);
            Add(newButton);
            Add(deleteButton);
            #endregion

            #region bind-button-events
            editButton.Clicked = () =>
            {
                if (lv.SelectedItem > -1)
                {
                    var settingsWindow = new RedisSettingsWindow(keys[lv.SelectedItem], _parent);
                    _parent.Add(settingsWindow);
                    Close();
                }
            };


            newButton.Clicked = () =>
            {
                var settingsWindow = new RedisSettingsWindow(_parent);
                _parent.Add(settingsWindow);
                Close();
            };

            deleteButton.Clicked = () =>
            {
                var res = MessageBox.ErrorQuery(60, 8, "Delete an instance", "Are you sure you want to delete the instance settings?\nThis cannot be undone", "Ok", "Cancel");
                if (res == 0)
                {
                    if (lv.SelectedItem > -1)
                    {
                        AppProvider.Delete(keys[lv.SelectedItem]);
                        var instancesWindow = new RedisInstancesWindow(_parent);
                        _parent.Add(instancesWindow);
                        Close();
                    }
                }
            };



            #endregion
        }

    }
}
