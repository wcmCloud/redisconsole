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


        public RedisEntryWindow(string serveritemKey, RedisKey? key, string redisEntryType, RecordTypeEnum recordtype, View parent) : base("Source: " + serveritemKey, 1)
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
                        Width = Dim.Fill(),
                        Height = Dim.Fill()
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
                        X = 1,
                        Y = 8
                    };
                    Add(saveButton);

                    var deleteButton = new Button("Delete")
                    {
                        X = Pos.Right(saveButton) + buttonSpacing,
                        Y = Pos.Top(saveButton)
                    };
                    Add(deleteButton);

                    var setTTLButton = new Button("Set TTL")
                    {
                        X = Pos.Right(saveButton) + buttonSpacing,
                        Y = Pos.Top(saveButton)
                    };
                    Add(setTTLButton);

                    var reloadButton = new Button("Reload")
                    {
                        X = Pos.Right(setTTLButton) + buttonSpacing,
                        Y = Pos.Top(setTTLButton)
                    };
                    Add(reloadButton);



                    var exitButton = new Button("eXit")
                    {
                        X = Pos.Right(reloadButton) + buttonSpacing,
                        Y = Pos.Top(reloadButton)
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

                    saveButton.Clicked = () =>
                    {
                        try
                        {
                            RedisStore store = new RedisStore(client);
                            bool res = false;
                            switch (redisDataType)
                            {
                                case RedisDataTypeEnum.String:
                                    res = store.Set(keyText.Text.ToString(), valueText.Text.ToString());

                                    break;
                                default:
                                    MessageBox.ErrorQuery(25, 12, "Error", "Not implemented yet", "OK");
                                    break;

                            }
                            if (res)
                                MessageBox.Query(25, 12, "Info", "Record Saved", "OK");
                            else
                                MessageBox.ErrorQuery(25, 12, "Error", "Record not saved", "OK");
                        }
                        catch(Exception ex)
                        {
                            Logger.LogException(ex);
                            MessageBox.ErrorQuery(25, 12, "Error", "Unkown Error " + ex.Message, "OK");
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
                        var entriesWindow = new RedisInstanceEntriesWindow(serverItemKey, _parent);
                        _parent.Add(entriesWindow);
                        Close();
                    };


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

