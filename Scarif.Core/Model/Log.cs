﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Scarif.Core.Model
{
    public class Log
    {
        public string AppId { get; set; }
        public App App { get; set; }
        [Key]
        public Guid LogId { get; set; }
        public string Component { get; set; }
        public string Severity { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public IList<Property> Properties { get; set; }
    }
}
