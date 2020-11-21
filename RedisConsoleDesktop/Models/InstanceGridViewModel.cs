using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class InstanceGridViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; }

        public InstanceGridViewModel(int id, string key)
        {
            Id = id;
            Key = key;
        }

    }
}
