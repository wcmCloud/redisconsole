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

        public RedisInstanceEntriesWindow(string itemKey, View parent) : base(itemKey + " - entries", 1)
        {

            client = AppProvider.Get(itemKey);
            _parent = parent;
            InitStyle();
            InitControls(client);

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

        private void InitControls(RedisClient r)
        {
            try
            {

                RedisStore store = new RedisStore(r);
                try
                {
                    var rediskeys = store.RedisServerKeys;
                    ListView lv = new ListView(rediskeys.Select(x => x.ToString()).ToList())
                    {
                        X = 1,
                        Y = 0,
                        Width = Dim.Percent(100),
                        Height = Dim.Fill() - 3
                    };
                    Add(lv);

                    #region buttons

                    var editButton = new Button("Edit", true)
                    {
                        X = Pos.Left(lv),
                        Y = Pos.Bottom(lv) + 1
                    };
                    Add(editButton);

                    var newButton = new Button("New")
                    {
                        X = Pos.Right(editButton) + 1,
                        Y = Pos.Top(editButton)
                    };
                    Add(newButton);

                    var deleteEntryButton = new Button("Del")
                    {
                        X = Pos.Right(newButton) + 1,
                        Y = Pos.Top(newButton)
                    };
                    Add(deleteEntryButton);

                    var deleteEntriesPatternButton = new Button("Del (pattern)")
                    {
                        X = Pos.Right(deleteEntryButton) + 1,
                        Y = Pos.Top(deleteEntryButton)
                    };
                    Add(deleteEntriesPatternButton);

                    var deleteEntriesRegexButton = new Button("Del (Regex)")
                    {
                        X = Pos.Right(deleteEntriesPatternButton) + 1,
                        Y = Pos.Top(deleteEntriesPatternButton)
                    };
                    Add(deleteEntriesRegexButton);

                    var flushDBButton = new Button("Flush db")
                    {
                        X = Pos.Right(deleteEntriesRegexButton) + 1,
                        Y = Pos.Top(deleteEntriesRegexButton)
                    };
                    Add(flushDBButton);

                    var flushServerButton = new Button("Flush Server (all dbs)")
                    {
                        X = Pos.Right(flushDBButton) + 1,
                        Y = Pos.Top(flushDBButton)
                    };
                    Add(flushServerButton);

                    var exitButton = new Button("Exit")
                    {
                        X = Pos.Right(flushServerButton) + 1,
                        Y = Pos.Top(flushServerButton)
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

                    editButton.Clicked = () =>
                    {
                        if (lv.SelectedItem > -1)
                        {
                            // var instanceWindow = new RedisInstanceEntriesWindow(keys[lv.SelectedItem], _parent);
                            // _parent.Add(instanceWindow);
                            // Close();
                        }
                    };
                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    var exitButton = new Button("Exit", true)
                    {
                        X = 1,
                        Y = 13,
                    };
                    Add(exitButton);
                    exitButton.Clicked = () =>
                    {
                        //OnExit?.Invoke();
                        var instancesWindow = new RedisInstancesWindow(_parent);
                        _parent.Add(instancesWindow);
                        Close();
                    };

                    MessageBox.ErrorQuery(25, 8, "Error", "Failed to connect to Redis Server", "OK");
                }





            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.ErrorQuery(25, 12, "Error", "Unkown Error " + ex.Message, "OK");

            }
        }
    }

}

