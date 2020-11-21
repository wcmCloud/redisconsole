using Redis.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class InstanceSettingsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public int Port { get; set; } = 6379;

        public string Auth { get; set; }

        public InstanceSettingsViewModel()
        {
            Host = "127.0.0.1";
            Port = 6379;
        }

        public InstanceSettingsViewModel(RedisClient client)
        {
            Id = client.Id;
            Name = client.Name;
            Host = client.Host;
            Port = 6379;
            Auth = client.Auth;
        }

    }
}
