﻿using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;

namespace ConsoleUI
{
    public class RedisInstanceEntriesWindow : Window
    {
        private readonly View _parent;
        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        public RedisInstanceEntriesWindow(string itemKey, View parent) : base(itemKey, 3)
        {
            client = AppProvider.Get(itemKey);
            _parent = parent;
            InitControls(client);
            InitStyle();
        }

        public void InitStyle()
        {
            X = Pos.Center();
            Width = Dim.Percent(95);
            Height = 23;
        }

        public void Close()
        {
            _parent?.Remove(this);
        }

        private void InitControls(RedisClient r)
        {

            RedisStore store = new RedisStore(r);
            var rediskeys = store.RedisServerKeys;

            ListView lv = new ListView(rediskeys.Select(x => x.ToString()).ToList())
            {
                X = 1,
                Y = 0,
                Width = Dim.Percent(100),
                Height = 12
            };
            Add(lv);

            #region buttons


            var exitButton = new Button("Exit")
            {
                X = Pos.Left(lv),
                Y = Pos.Bottom(lv) + 1
            };

            Add(exitButton);

            #endregion

            #region bind-button-events
        

            exitButton.Clicked = () =>
            {
                //OnExit?.Invoke();
                var instancesWindow = new RedisInstancesWindow(_parent);
                _parent.Add(instancesWindow);
                Close();
            };


            #endregion
        }
    }

}

