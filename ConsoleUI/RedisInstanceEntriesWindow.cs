using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;
using StackExchange.Redis;

namespace ConsoleUI
{
    public class RedisInstanceEntriesWindow : ConsoleWindowBase
    {
        private const int buttonSpacing = 1;
        private const string typeSeparator = "::";

        //private readonly View _parent;
        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        private ListView itemList { get; set; }

        private string filterText { get; set; }

        private string serverItemKey { get; set; }

        private IEnumerable<RedisKey> Keys { get; set; }

        public RedisInstanceEntriesWindow(string serveritemKey, string filtertext = null) : base(serveritemKey + " Filter:" + ((filtertext == null || filtertext == "") ? "inactive" : filtertext))
        {
            serverItemKey = serveritemKey;
            filterText = filtertext;
            client = AppProvider.Get(serveritemKey);
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

                    var filterText = new TextField(r == null ? "" : "")
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill()
                    };
                    Add(filterText);
                    var filterButton = new Button("Filter", true)
                    {
                        X = Pos.Left(filterText),
                        Y = Pos.Top(filterText) + 1

                    };
                    Add(filterButton);
                    filterButton.Clicked = () =>
                    {
                        var tframe = Application.Top.Frame;
                        var ntop = new Toplevel(tframe);
                        var instanceWindow = new RedisInstanceEntriesWindow(serverItemKey, filterText.Text.ToString());
                        Close();
                        ntop.Add(instanceWindow);
                        ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
                        Application.Run(ntop);
                    };


                    string pattern = "*";
                    if (this.filterText != null && this.filterText != "")
                        pattern = "*" + this.filterText + "*";
                    Keys = store.RedisServerKeys(pattern);



                    ListView lv = new ListView(Keys.Select(x => x.ToString() + typeSeparator + store.GetKeyType(x.ToString())).ToList())
                    {
                        X = 1,
                        Y = 3,
                        Width = Dim.Percent(100),
                        Height = Dim.Fill() - 3,
                        AllowsMultipleSelection = false
                    };
                    Add(lv);
                    itemList = lv;
                    #region buttons

                    var editButton = new Button("Edit", false)
                    {
                        X = Pos.Left(lv),
                        Y = Pos.Bottom(lv) + 1
                    };
                    Add(editButton);

                    var newButton = new Button("New")
                    {
                        X = Pos.Right(editButton) + buttonSpacing,
                        Y = Pos.Top(editButton),
                    };
                    Add(newButton);

                    var deleteEntryButton = new Button("Del selected")
                    {
                        X = Pos.Right(newButton) + buttonSpacing,
                        Y = Pos.Top(newButton)
                    };
                    Add(deleteEntryButton);

                    var deleteEntriesPatternButton = new Button("del fiLtered")
                    {
                        X = Pos.Right(deleteEntryButton) + buttonSpacing,
                        Y = Pos.Top(deleteEntryButton)
                    };
                    Add(deleteEntriesPatternButton);
                 
                    var flushAllDbsButton = new Button("flush All dbs")
                    {
                        X = Pos.Right(deleteEntriesPatternButton) + buttonSpacing,
                        Y = Pos.Top(deleteEntriesPatternButton)
                    };
                    Add(flushAllDbsButton);

                    var exitButton = new Button("eXit")
                    {
                        X = Pos.Right(flushAllDbsButton) + buttonSpacing,
                        Y = Pos.Top(flushAllDbsButton)
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
                        ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
                        Application.Run(ntop);
                    };

                    editButton.Clicked = () =>
                    {
                        if (lv.SelectedItem > -1)
                        {
                            var tframe = Application.Top.Frame;
                            var ntop = new Toplevel(tframe);
                            var selectedKey = Keys.ToArray()[itemList.SelectedItem];
                            var enrtyWindow = new RedisEntryWindow(serverItemKey, selectedKey, store.GetKeyType(selectedKey.ToString()), RedisEntryWindow.RecordTypeEnum.Edit);
                            Close();
                            ntop.Add(enrtyWindow);
                            ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
                            Application.Run(ntop);
                        }
                    };

                    deleteEntryButton.Clicked = () =>
                    {
                        var res = MessageBox.ErrorQuery(70, 8, "Delete seleted row", "Are you sure you want to proceed?\nThis cannot be undone", "Ok", "Cancel");
                        if (res == 0)
                        {
                            if (lv.SelectedItem > -1)
                            {
                                store.Remove(Keys.ToArray()[itemList.SelectedItem]);

                                var tframe = Application.Top.Frame;
                                var ntop = new Toplevel(tframe);
                                var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                                Close();
                                ntop.Add(instancesWindow);
                                ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
                                Application.Run(ntop);
                            }
                        }
                    };

                    flushAllDbsButton.Clicked = () =>
                    {
                        var res = MessageBox.ErrorQuery(70, 8, "Flush Server (all dbs)", "Are you sure you want to proceed?\nThis cannot be undone", "Ok", "Cancel");
                        if (res == 0)
                        {
                            if (lv.SelectedItem > -1)
                            {
                                store.FlushAllDbs();

                                var tframe = Application.Top.Frame;
                                var ntop = new Toplevel(tframe);
                                var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                                Close();
                                ntop.Add(instancesWindow);
                                ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
                                Application.Run(ntop);
                            }
                        }
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
                        ntop.Add(MenuProvider.GetMenu(AppProvider.Configuration));
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

