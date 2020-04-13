using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Core
{
    [JsonObject("AppConfiguration")]
    public class AppConfiguration
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
