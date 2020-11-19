using Scarif.Core.Model;
using Scarif.Protobuf;
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
        bool InsertLog(LogMessage message);
        IEnumerable<LogMessage> AllLogsForApp(string appName, bool[] severities, string? componentFilter);
        void InsertInternalLog(LogMessage message);
        void InsertInternalLog(string component, string severity, string message);
    }
}
