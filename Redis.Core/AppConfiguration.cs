﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Core
{
    [JsonObject("appConfiguration")]
    public class AppConfiguration
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
        
        [JsonProperty("author")]
        public string Author { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
