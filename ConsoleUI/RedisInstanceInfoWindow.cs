using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;

namespace ConsoleUI
{
    public class RedisInstanceInfoWindow : Window
    {
        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        public RedisInstanceInfoWindow(string itemKey) : base(itemKey + " - info/stats", 1)
        {

            client = AppProvider.Get(itemKey);
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
            Application.Top.Clear();
            Application.Top?.Remove(this);
        }

        private void InitControls(RedisClient r)
        {
            try
            {

                RedisStore store = new RedisStore(r);
                try
                {
                    var rediskeys = store.GenerateServerInfoDictionary();
                    ListView lv = new ListView(rediskeys.Select(x => x.Value != "" ? x.Key + ": " + x.Value : x.Key).ToList())
                    {
                        X = 1,
                        Y = 0,
                        Width = Dim.Percent(100),
                        Height = Dim.Fill() - 3
                    };
                    Add(lv);

                    #region buttons

                    var exitButton = new Button("eXit", true)
                    {
                        X = Pos.Left(lv),
                        Y = Pos.Bottom(lv) + 1
                    };
                    Add(exitButton);
                    #endregion
                    #region bind-button-events


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
                catch (Exception ex)
                {
                    Logger.LogException(ex);

                    var exitButton = new Button("eXit", true)
                    {
                        X = 1,
                        Y = 13,
                    };
                    Add(exitButton);
                    exitButton.Clicked = () =>
                    {
                        var tframe = Application.Top.Frame;
                        var ntop = new Toplevel(tframe);
                        var instancesWindow = new RedisInstancesWindow();
                        Close();
                        ntop.Add(instancesWindow);
                        Application.Run(ntop);
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

