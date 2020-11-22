using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Models
{
    public class DataGridViewModel
    {
        public int InstanceId { get; set; }
        public string Key { get; set; }
        public string RecordType { get; set; }
        public TimeSpan? TTL { get; set; }
        public string DataPreview { get; set; }


        public DataGridViewModel(int instanceId, string key, string recordType, TimeSpan? ttl, string datapreview)
        {
            InstanceId = instanceId;
            Key = key;
            RecordType = recordType;
            TTL = ttl;
            DataPreview = datapreview;
        }

    }
}
