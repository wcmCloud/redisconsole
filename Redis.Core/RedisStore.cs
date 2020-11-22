using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Redis.Core
{

    public class RedisStore
    {
        private readonly Lazy<ConnectionMultiplexer> LazyConnection;

        private readonly RedisClient Client;

        public RedisStore(RedisClient client)
        {
            Client = client;
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { client.Host + ":" + client.Port.ToString() },
                Password = client.Auth,
                ConnectRetry = 5,
                AllowAdmin = true
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public ConnectionMultiplexer Connection => LazyConnection.Value;

        public IDatabase RedisCache(int db = -1) => Connection.GetDatabase(db);

        public IServer RedisServer => Connection.GetServer(Client.Host, Client.Port);

        public IEnumerable<RedisKey> RedisServerKeys(string patt = "*") => RedisServer.Keys(pattern: patt);

        public IEnumerable<ClientInfo> RedisClientList => RedisServer.ClientList(CommandFlags.None).AsEnumerable<ClientInfo>();

        public int RedisdbCount => RedisServer.DatabaseCount;

        public long RediskeyCount => RedisServer.DatabaseSize();

        public RedisFeatures RedisGetFeatures => RedisServer.Features;

        public ServerCounters RedisGetCounters => RedisServer.GetCounters();

        public Version RedisVersion => RedisServer.Version;

        public Dictionary<string, string> GenerateServerInfoDictionary()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            res.AddToDictionary<string, string>("Database Count", this.RedisdbCount.ToString());
            res.AddToDictionary<string, string>("Version", this.RedisVersion.ToString());

            res.AddToDictionary<string, string>("Item Count", this.RediskeyCount.ToString());
            res.AddToDictionary<string, string>("EndPoint", this.RedisGetCounters.EndPoint.ToString());
            res.AddToDictionary<string, string>("Interactive counters", this.RedisGetCounters.Interactive.ToString());
            res.AddToDictionary<string, string>("Other counters", this.RedisGetCounters.Other.ToString());
            res.AddToDictionary<string, string>("Subscription counters", this.RedisGetCounters.Subscription.ToString());

            res.AddToDictionary<string, string>("Client List", this.RedisClientList.Count().ToString());
            foreach (var c in this.RedisClientList)
            {
                res.AddToDictionary<string, string>(" - " + c.ToString(), "");
            }

            var features = this.RedisGetFeatures.ToString().Trim().Split(Environment.NewLine);
            int counter = 0;
            foreach (var f in features)
            {
                if (counter == 0)
                    res.AddToDictionary<string, string>(f, "");
                else
                    res.AddToDictionary<string, string>(" - " + f, "");
                counter++;
            }

            return res;
        }

        public void FlushAllDbs()
        {
            var endpoints = Connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public void FlushDb(int dbidx = 0)
        {
            this.RedisServer.FlushDatabase(dbidx);
        }

        public string Get(string key, int db = -1)
        {
            return this.RedisCache(db).StringGet(key, CommandFlags.None);
        }

        #region List operations
        public RedisValue GetListbyIndex(string key, int index = 0, int db = -1)
        {
            return this.RedisCache(db).ListGetByIndex(key, index, CommandFlags.None);
        }

        public RedisValue[] GetListValuesbyIndex(string key, int start = 0, int stop = -1, int db = -1)
        {
            return this.RedisCache(db).ListRange(key, start, stop, CommandFlags.None);
        }

        public void ListRightPush(string key, string val, int db = -1)
        {
            this.RedisCache(db).ListRightPush(key, val);
        }

        public void ListLeftPush(string key, string val, int db = -1)
        {
            this.RedisCache(db).ListLeftPush(key, val);
        }

        public void ListSetByIndex(string key, int index, string val, int db = -1)
        {
            this.RedisCache(db).ListSetByIndex(key, index, val);
        }

        public void ListRemove(string key, string val, int db = -1)
        {
            this.RedisCache(db).ListRemove(key, val);
        }
        #endregion


        #region Set operations
        public RedisValue[] GetSetMembers(string key, int db = -1)
        {
            return this.RedisCache(db).SetMembers(key, CommandFlags.None);
        }

        public void SetAdd(string key, string val, int db = -1)
        {
            this.RedisCache(db).SetAdd(key, val);
        }


        public void SetRemove(string key, string val, int db = -1)
        {
            this.RedisCache(db).SetRemove(key, val);
        }
        #endregion

        #region SortedSet operations
        public RedisValue[] GetSortedSetMembersByScore(string key, int db = -1)
        {
            return this.RedisCache(db).SortedSetRangeByScore(key);
        }

        public IEnumerable<SortedSetEntry> GetSortedSetScan(string key, int db = -1)
        {
            return this.RedisCache(db).SortedSetScan(key);
        }



        public void SortedSetAdd(string key, string val, double score, int db = -1)
        {
            this.RedisCache(db).SortedSetAdd(key, val, score);
        }

        public void SortedSetRemove(string key, string val, int db = -1)
        {
            this.RedisCache(db).SortedSetRemove(key, val);
        }
        #endregion


        public TimeSpan? GetTTL(string key, int db = -1)
        {
            return this.RedisCache(db).KeyTimeToLive(key, CommandFlags.None);
        }


        public string GetKeyType(string key, int db = -1)
        {
            return this.RedisCache(db).KeyType(key, CommandFlags.None).ToString();
        }

        public bool Exists(string key, int db = -1)
        {
            return this.RedisCache(db).KeyExists(key);
        }

        public bool Set(string key, string value, int db = -1)
        {
            return this.RedisCache(db).StringSet(key, value);
        }

        public bool Remove(string key, int db = -1)
        {
            return this.RedisCache(db).KeyDelete(key);
        }

        public IEnumerable<HashEntry> GetHashes(string key, int db = -1)
        {
            return this.RedisCache(db).HashGetAll(new RedisKey(key)).AsEnumerable();
        }

        public static RedisDataTypeEnum GetDataType(string dataType)
        {
            switch (dataType)
            {
                case "String":
                    return RedisDataTypeEnum.String;
                case "Stream":
                    return RedisDataTypeEnum.Stream;
                case "SortedSet":
                    return RedisDataTypeEnum.SortedSet;
                case "Set":
                    return RedisDataTypeEnum.Set;
                case "List":
                    return RedisDataTypeEnum.List;
                case "HyperLog":
                    return RedisDataTypeEnum.HyperLog;
                case "Hash":
                    return RedisDataTypeEnum.Hash;
                case "BitArray":
                    return RedisDataTypeEnum.BitArray;
                default:
                    throw new NotImplementedException("Uknown data type:" + dataType);
            }
        }

    }

}
