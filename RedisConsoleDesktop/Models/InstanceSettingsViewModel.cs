using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class InstanceSettingsViewModel
    {

        public string Name { get; set; }

        public string Host { get; set; }

        public int Port { get; set; } = 6379;

        public string Auth { get; set; }

    }
}
