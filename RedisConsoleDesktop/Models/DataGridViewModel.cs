using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redis.Core;

namespace RedisConsoleDesktop.Models
{
    public class DataGridViewModel
    {
        public int InstanceId { get; set; }
        public string Key { get; set; }
        public string RecordType { get; set; }
        public TimeSpan? TTL { get; set; }
        public string DataPreview { get; set; }

        public RedisDataTypeEnum RedisType { get; private set; }


        public DataGridViewModel(int instanceId, string key, string recordType, TimeSpan? ttl, string datapreview)
        {
            InstanceId = instanceId;
            Key = key;
            RecordType = recordType;
            TTL = ttl;
            DataPreview = datapreview;
            RedisType = Enum.Parse<RedisDataTypeEnum>(recordType);
        }

    }
}
