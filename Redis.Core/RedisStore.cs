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

        public IDatabase RedisCache => Connection.GetDatabase();

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
            this.RedisServer.FlushAllDatabases(CommandFlags.None);
        }

        public void FlushDb(int dbidx = 0)
        {
            this.RedisServer.FlushDatabase(dbidx);
        }

        public string Get(string key)
        {
            return this.RedisCache.StringGet(key, CommandFlags.None);
        }

        public string GetKeyType(string key)
        {
            return this.RedisCache.KeyType(key, CommandFlags.None).ToString();
        }

        public bool Set(string key, string value)
        {
            return this.RedisCache.StringSet(key, value);
        }

        public IEnumerable<HashEntry> GetHashes(string key)
        {
            return this.RedisCache.HashGetAll(new RedisKey(key)).AsEnumerable();
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
