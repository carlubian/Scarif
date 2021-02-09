using System;

namespace Scarif.Core.Model
{
    public class Property
    {
        public long LogId { get; set; }
        public Log Log { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
