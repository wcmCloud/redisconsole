using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;
using StackExchange.Redis;
using System.Threading.Tasks;
using Attribute = Terminal.Gui.Attribute;

namespace ConsoleUI
{
    public class RedisEntryWindow : Window
    {
        public enum RecordTypeEnum
        {
            New,
            Edit
        }

        private RecordTypeEnum recordType { get; set; }

        private const int buttonSpacing = 1;

        private readonly View _parent;
        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        private string serverItemKey { get; set; }
        private RedisKey? itemKey { get; set; }

        private RedisDataTypeEnum redisDataType { get; set; }


        public RedisEntryWindow(string serveritemKey, RedisKey? key, string redisEntryType, RecordTypeEnum recordtype, View parent) : base(serveritemKey, 1)
        {
            serverItemKey = serveritemKey;
            itemKey = key;
            redisDataType = RedisStore.GetDataType(redisEntryType);
            recordType = recordType;
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




                    var keyLabel = new Label("Key")
                    {
                        X = 0,
                        Y = 0
                    };
                    Add(keyLabel);
                    var keyText = new TextField(itemKey.ToStringSafe())
                    {
                        X = 5,
                        Y = 0,
                        Width = Dim.Fill()
                    };
                    Add(keyText);


                    var valueText = new TextView()
                    {
                        X = 0,
                        Y = 2,
                        Width = Dim.Fill(),
                        Height = 5,
                        ColorScheme = Colors.Menu,
                    };
                    valueText.Text = store.Get(itemKey.ToString());
                    Add(valueText);


                    #region buttons

                    var saveButton = new Button("Save", true)
                    {
                        X = 2,
                        Y = Pos.Bottom(valueText) + 2
                    };
                    Add(saveButton);



                    var exitButton = new Button("eXit")
                    {
                        X = Pos.Right(saveButton) + buttonSpacing,
                        Y = Pos.Top(saveButton)
                    };
                    Add(exitButton);
                    #endregion
                    #region bind-button-events


                    exitButton.Clicked = () =>
                    {
                        //OnExit?.Invoke();
                        var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey, _parent);
                        _parent.Add(instancesWindow);
                        Close();
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

