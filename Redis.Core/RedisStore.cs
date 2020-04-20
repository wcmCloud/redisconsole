using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

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
                Password = client.Auth
            };

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public ConnectionMultiplexer Connection => LazyConnection.Value;

        public IDatabase RedisCache => Connection.GetDatabase();

        public IServer RedisServer => Connection.GetServer(Client.Host, Client.Port);

        public IEnumerable<RedisKey> RedisServerKeys => RedisServer.Keys(pattern: "*");

        public object Get(string key)
        {

            return null;
        }
    }
}
