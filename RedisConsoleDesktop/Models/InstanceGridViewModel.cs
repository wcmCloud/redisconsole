using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class InstanceGridViewModel
    {

        public string Key { get; set; }

        public InstanceGridViewModel(string key)
        {
            Key = key;
        }

    }
}
