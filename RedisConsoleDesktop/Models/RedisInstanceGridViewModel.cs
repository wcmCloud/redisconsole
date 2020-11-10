using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class RedisInstanceGridViewModel
    {

        public string Key { get; set; }

        public RedisInstanceGridViewModel(string key)
        {
            Key = key;
        }

    }
}
