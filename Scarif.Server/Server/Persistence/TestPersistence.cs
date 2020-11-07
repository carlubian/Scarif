using Scarif.Core.Model;
using Scarif.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Persistence
{
    public class TestPersistence : IPersistence
    {
        private static IDictionary<string, IEnumerable<LogMessage>> _Storage = new Dictionary<string, IEnumerable<LogMessage>>();

        public IEnumerable<ScarifApp> GetAllApps()
        {
            yield return new ScarifApp
            {
                Name = "SARAH",
                Url = "sarah"
            };
            yield return new ScarifApp
            {
                Name = "TimeChef",
                Url = "timechef"
            };
            yield return new ScarifApp
            {
                Name = "Agenda 2.0",
                Url = "agenda2"
            };
            yield return new ScarifApp
            {
                Name = "Test",
                Url = "test"
            };
        }

        public string AppNameFromUrl(string appUrl)
        {
            switch (appUrl)
            {
                case "sarah":
                    return "SARAH";
                case "timechef":
                    return "TimeChef";
                case "agenda2":
                    return "Agenda 2.0";
                case "test":
                    return "Test";
                default:
                    return "[Unknown]";
            }
        }

        public void InsertLog(LogMessage message)
        {
            var app = message.App;

            if (!_Storage.ContainsKey(app))
                _Storage.Add(app, new List<LogMessage>());

            (_Storage[app] as List<LogMessage>).Add(message);
        }

        public IEnumerable<LogMessage> AllLogsForApp(string appName)
        {
            if (_Storage.ContainsKey(appName))
                return _Storage[appName];

            return Enumerable.Empty<LogMessage>();
        }
    }
}
