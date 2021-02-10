using System;
using System.Text.Json.Serialization;

namespace Scarif.Core.Model
{
    public class Property
    {
        public long LogId { get; set; }
        [JsonIgnore]
        public Log Log { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
