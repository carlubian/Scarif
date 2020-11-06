using Scarif.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Persistence
{
    public interface IPersistence
    {
        IEnumerable<ScarifApp> GetAllApps();
        string AppNameFromUrl(string appUrl);
    }
}
