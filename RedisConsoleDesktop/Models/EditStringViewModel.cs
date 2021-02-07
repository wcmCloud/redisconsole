using System;
namespace RedisConsoleDesktop.Models
{
    public class EditStringViewModel
    {
        public int InstanceId { get; set; }
        public string InstanceName { get; set; }
        public string Key { get; set; }
        public string RecordType { get; set; }
        public TimeSpan? TTL { get; set; }
        public string Value { get; set; }



        public EditStringViewModel()
        {




        }
    }
}
