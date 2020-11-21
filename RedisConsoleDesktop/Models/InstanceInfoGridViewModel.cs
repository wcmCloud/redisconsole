using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    
    public class InstanceInfoGridViewModel
    {

        public string Key { get; set; }
        public string Value { get; set; }


        public InstanceInfoGridViewModel(KeyValuePair<string, string> item)
        {
            if ((item.Value == null) || (item.Value == ""))
            {
                Key = "";
                Value = item.Key;

            }
            else
            {
                Key = item.Key;
                Value = item.Value;
            }
        }

    }
}
