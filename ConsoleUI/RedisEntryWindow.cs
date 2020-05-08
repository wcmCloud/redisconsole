using Redis.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;
using System.Linq;
using StackExchange.Redis;
using System.Threading.Tasks;
using Attribute = Terminal.Gui.Attribute;
using System.Runtime.CompilerServices;

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

        private const int messageBoxHeight = 7;
        private int messageBoxWidth
        {
            get { return 60; }
        }

        public Action<(string name, string host, int port, string auth)> OnSave { get; set; }
        public Action OnExit { get; set; }

        private RedisClient client;

        private string serverItemKey { get; set; }
        private RedisKey? itemKey { get; set; }

        private RedisDataTypeEnum redisDataType { get; set; }


        public RedisEntryWindow(string serveritemKey, RedisKey? key, string redisEntryType, RecordTypeEnum recordtype) : base("Source: " + serveritemKey, 1)
        {
            serverItemKey = serveritemKey;
            itemKey = key;
            redisDataType = RedisStore.GetDataType(redisEntryType);
            recordType = recordType;
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
                    #region buttons

                    var saveButton = new Button("Save", true)
                    {
                        X = 0,
                        Y = 0
                    };
                    Add(saveButton);

                    var deleteButton = new Button("Delete")
                    {
                        X = Pos.Right(saveButton) + buttonSpacing,
                        Y = Pos.Top(saveButton)
                    };
                    Add(deleteButton);

                    var exitButton = new Button("eXit")
                    {
                        X = Pos.Right(deleteButton) + buttonSpacing,
                        Y = Pos.Top(deleteButton)
                    };
                    Add(exitButton);
                    #endregion

                    var keyLabel = new Label("Key")
                    {
                        X = 0,
                        Y = exitButton.Y + 2
                    };
                    Add(keyLabel);
                    var keyText = new TextField(itemKey.ToStringSafe())
                    {
                        X = 4,
                        Y = exitButton.Y + 2,
                        Width = Dim.Fill()
                    };
                    Add(keyText);

                    var ttl = store.GetTTL(itemKey);
                    var ttlLabel = new Label("TTL:" + (ttl.HasValue ? ttl.ToStringOrDefault("-1") : "none"))
                    {
                        X = 0,
                        Y = keyText.Y + 1
                    };
                    Add(ttlLabel);


                    var valueLabel = new Label("Value")
                    {
                        X = 0,
                        Y = ttlLabel.Y + 1
                    };
                    Add(valueLabel);
                    var valueText = new TextView()
                    {
                        X = 0,
                        Y = ttlLabel.Y + 2,
                        Width = Dim.Fill(),
                        Height = Dim.Fill(),
                        ColorScheme = Colors.Menu,
                    };
                    valueText.Text = store.Get(itemKey.ToString());
                    Add(valueText);

                    #region bind-button-events


                    exitButton.Clicked = () =>
                    {
                        var tframe = Application.Top.Frame;
                        var ntop = new Toplevel(tframe);
                        var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                        Close();
                        ntop.Add(instancesWindow);
                        Application.Run(ntop);
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
                                    MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Not implemented yet", "OK");
                                    break;

                            }
                            if (res)
                            {
                                MessageBox.Query(messageBoxWidth, messageBoxHeight, "Info", "Record Saved", "OK");
                                var tframe = Application.Top.Frame;
                                var ntop = new Toplevel(tframe);
                                var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                                var enrtyWindow = new RedisEntryWindow(serverItemKey, keyText.Text.ToString(), store.GetKeyType(keyText.Text.ToString()), RedisEntryWindow.RecordTypeEnum.Edit);
                                Close();
                                ntop.Add(enrtyWindow);
                                Application.Run(ntop);
                            }
                            else
                                MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Record not saved", "OK");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                            MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Unkown Error " + ex.Message, "OK");
                        }
                    };

                    deleteButton.Clicked = () =>
                    {
                        var resx = MessageBox.ErrorQuery(60, 8, "Delete the entry?", "Are you sure you want to delete the entry?\nThis cannot be undone", "Ok", "Cancel");
                        if (resx == 0)
                        {
                            try
                            {
                                RedisStore store = new RedisStore(client);
                                bool res = false;
                                switch (redisDataType)
                                {
                                    case RedisDataTypeEnum.String:
                                        res = store.Remove(keyText.Text.ToString());

                                        break;
                                    default:
                                        MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Not implemented yet", "OK");
                                        break;

                                }
                                if (res)
                                    MessageBox.Query(messageBoxWidth, messageBoxHeight, "Info", "Record Deleted", "OK");
                                else
                                    MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Record not deleted", "OK");
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex);
                                MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Unkown Error " + ex.Message, "OK");
                            }
                            var tframe = Application.Top.Frame;
                            var ntop = new Toplevel(tframe);
                            var instancesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                            Close();
                            ntop.Add(instancesWindow);
                            Application.Run(ntop);
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
                        var entriesWindow = new RedisInstanceEntriesWindow(serverItemKey);
                        Close();
                        ntop.Add(entriesWindow);
                        Application.Run(ntop);
                    };


                }





            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.ErrorQuery(messageBoxWidth, messageBoxHeight, "Error", "Unkown Error " + ex.Message, "OK");

            }
        }
    }

}

