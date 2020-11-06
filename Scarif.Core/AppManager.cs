using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scarif.Core
{
    public static class AppManager
    {
        public static string AppUrlFromName(string name)
        {
            return name.ToLowerInvariant()
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(",", "");
        }
    }
}
