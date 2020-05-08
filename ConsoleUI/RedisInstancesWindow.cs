using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
namespace ConsoleUI
{
    public class RedisInstancesWindow : Window
    {
        private const int buttonSpacing = 2;
        private readonly View _parent;
        //public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        //public Action OnExit { get; set; }
        private List<string> keys;

        public RedisInstancesWindow(View parent) : base("Redis Instances", 2)
        {
            _parent = parent;
            InitStyle();
            InitControls();
            _parent.BringSubviewToFront(this);
            try
            {
              //  Application.Top?.SetFocus(this);
            }catch(Exception ex) { };
          //  var xx = Application.Top.Focused;
        }
        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(100);
            Height = Dim.Fill();
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
                Height = Dim.Fill() - 3
            };
            Add(lv);

            #region buttons
            var connectButton = new Button("Connect", true)
            {
                X = Pos.Left(lv),
                Y = Pos.Bottom(lv) + 2
            };
            Add(connectButton);

            var infoButton = new Button("Info/Stats")
            {
                X = Pos.Right(connectButton) + buttonSpacing,
                Y = Pos.Top(connectButton)
            };
            Add(infoButton);

            var editButton = new Button("Edit")
            {
                X = Pos.Right(infoButton) + buttonSpacing,
                Y = Pos.Top(infoButton)
            };
            Add(editButton);

            var newButton = new Button("New")
            {
                X = Pos.Right(editButton) + buttonSpacing,
                Y = Pos.Top(editButton)
            };
            Add(newButton);

            var deleteButton = new Button("Delete")
            {
                X = Pos.Right(newButton) + buttonSpacing,
                Y = Pos.Top(newButton)
            };
            Add(deleteButton);

            #endregion

            #region bind-button-events
            connectButton.Clicked = () =>
            {
                if (lv.SelectedItem > -1)
                {
                    var instanceWindow = new RedisInstanceEntriesWindow(keys[lv.SelectedItem], _parent);
                    _parent.Add(instanceWindow);
                    Close();
                }
            };

            infoButton.Clicked = () =>
            {
                if (lv.SelectedItem > -1)
                {
                    var instanceWindow = new RedisInstanceInfoWindow(keys[lv.SelectedItem], _parent);
                    _parent.Add(instanceWindow);
                    Close();
                }
            };

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
