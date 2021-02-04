using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Scarif.Core.Model
{
    public class App
    {
        [Key]
        public string AppId { get; set; }
        public string AppName { get; set; }
        public IList<Log> Logs { get; set; }
    }
}
