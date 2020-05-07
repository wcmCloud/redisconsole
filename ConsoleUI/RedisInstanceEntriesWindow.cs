using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;
using StackExchange.Redis;

namespace ConsoleUI
{
    public class RedisInstanceEntriesWindow : Window
    {
        private const int buttonSpacing = 1;
        private const string typeSeparator = "::";

        private readonly View _parent;
        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        private ListView itemList { get; set; }

        private string filterText { get; set; }

        private string serverItemKey { get; set; }

        private IEnumerable<RedisKey> Keys { get; set; }

        public RedisInstanceEntriesWindow(string serveritemKey, View parent, string filtertext = null) : base(serveritemKey + " Filter:" + ((filtertext == null || filtertext == "") ? "inactive" : filtertext), 1)
        {
            serverItemKey = serveritemKey;
            filterText = filtertext;
            client = AppProvider.Get(serveritemKey);
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
                        var instanceWindow = new RedisInstanceEntriesWindow(serverItemKey, _parent, filterText.Text.ToString());
                        _parent.Add(instanceWindow);
                        Close();
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
                        AllowsMultipleSelection = true
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
                        Y = Pos.Top(editButton)
                    };
                    Add(newButton);

                    var deleteEntryButton = new Button("Del Selected")
                    {
                        X = Pos.Right(newButton) + buttonSpacing,
                        Y = Pos.Top(newButton)
                    };
                    Add(deleteEntryButton);

                    var deleteEntriesPatternButton = new Button("Del (all)")
                    {
                        X = Pos.Right(deleteEntryButton) + buttonSpacing,
                        Y = Pos.Top(deleteEntryButton)
                    };
                    Add(deleteEntriesPatternButton);

                    var flushDBButton = new Button("Flush db")
                    {
                        X = Pos.Right(deleteEntriesPatternButton) + buttonSpacing,
                        Y = Pos.Top(deleteEntriesPatternButton)
                    };
                    Add(flushDBButton);

                    var flushServerButton = new Button("Flush Server (all dbs)")
                    {
                        X = Pos.Right(flushDBButton) + buttonSpacing,
                        Y = Pos.Top(flushDBButton)
                    };
                    Add(flushServerButton);

                    var exitButton = new Button("eXit")
                    {
                        X = Pos.Right(flushServerButton) + buttonSpacing,
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
                            var selectedKey = Keys.ToArray()[itemList.SelectedItem];
                            var enrtyWindow = new RedisEntryWindow(serverItemKey, selectedKey, store.GetKeyType(selectedKey.ToString()), RedisEntryWindow.RecordTypeEnum.Edit, _parent);
                            _parent.Add(enrtyWindow);
                            Close();
                        }
                    };

                    flushServerButton.Clicked = () =>
                    {
                        var res = MessageBox.ErrorQuery(70, 8, "Flush Server (all dbs)", "Are you sure you want to proceed?\nThis cannot be undone", "Ok", "Cancel");
                        if (res == 0)
                        {
                            if (lv.SelectedItem > -1)
                            {
                                store.FlushAllDbs();
                                var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey, _parent);
                                _parent.Add(instancesWindow);
                                Close();
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

