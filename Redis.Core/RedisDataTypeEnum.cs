using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Core
{
    public enum RedisDataTypeEnum
    {
        String,
        List,
        Set,
        SortedSet,
        Hash,
        BitArray,
        HyperLog,
        Stream
    }
}
