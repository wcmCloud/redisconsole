﻿using RedisConsoleDesktop.Core;
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
        public string VClientTemplate { get; set; }



        public InstanceInfoGridViewModel(KeyValuePair<string, string> item)
        {
            if (item.Key.StartsWith(" - ") && ((item.Value == null) || (item.Value == "")))
            {
                Key = "";
                Value = item.Key?.Replace(" - ", "");
                VClientTemplate = "";
                if (Value.ToLower().Contains("true"))
                    VClientTemplate = "<span class='" + KendoBadgeHelpers.MediumRoundedBadgePrimary + "k-badge-info" + "'>" + Value + "</span>";
                else if (Value.ToLower().Contains("false"))
                    VClientTemplate = "<span class='" + KendoBadgeHelpers.MediumRoundedBadgeWarning + "k-badge-error" + "'>" + Value + "</span>";
                else
                    VClientTemplate = "<span>" + Value + "</span>";
            }
            else
            {
                Key = item.Key;
                Value = item.Value;
                VClientTemplate = "<span>" + Value + "</span>";
            }
        }

    }
}