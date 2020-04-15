using Hanssens.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Redis.Core
{
    public static class AppProvider
    {
        public static AppConfiguration Configuration { get; set; }

        private static LocalStorage storage;
        public static LocalStorage Storage
        {
            get
            {
                if (storage == null)
                {
                    // setup a configuration with encryption enabled (defaults to 'false')
                    // note that adding EncryptionSalt is optional, but recommended
                    var config = new LocalStorageConfiguration()
                    {
                        EnableEncryption = Configuration.EnableEncryption,
                        EncryptionSalt = Configuration.EncryptionPass,
                        Filename = Configuration.Filename
                    };

                    // initialize LocalStorage with a password of your choice
                    var encryptedStorage = new LocalStorage(config, Configuration.EncryptionPass);

                    storage = encryptedStorage;
                    //Storage.Persist();
                    storage.Load();
                }
                return storage;
            }
        }

        public static void Store(RedisClient redisClient)
        {

            Storage.Store<RedisClient>(redisClient.Name, redisClient);
            Storage.Persist();

        }

        public static RedisClient Get(string redisClientKey)
        {
            return Storage.Get<RedisClient>(redisClientKey);
        }

        public static int GetCount()
        {
            return Storage.Count;
        }

        public static List<string> GetKeys()
        {
            return Storage.Keys().OrderBy(x => x).ToList<string>();
        }

    }
}
