using Google.Protobuf.WellKnownTypes;
using Scarif.Core;
using Scarif.Core.Model;
using Scarif.Protobuf;
using Scarif.Server.Server.Core;
using Scarif.Server.Server.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Persistence
{
    public class SQLitePersistence : IPersistence
    {
        private IDictionary<string, SQLiteAdapter> Adapters = new Dictionary<string, SQLiteAdapter>();

        public SQLitePersistence()
        {
            var logDir = Path.Combine(Environment.CurrentDirectory, "ScarifApps");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            // Load Scarif logs
            Adapters.Add("Scarif", SQLiteAdapter.CreateInternalAdapter());

            // Get all app files and open as SQLite
            var appUrls = Directory.EnumerateFiles(logDir, "*.scarif");
            foreach (var appUrl in appUrls)
            {
                var fileName = new FileInfo(appUrl).Name.Replace(".scarif", "");
                if (fileName is "Internal.Logs")
                    continue;

                InsertInternalLog("SQLitePersistence:Ctor", "Info", $"Located app in {appUrl}: {fileName}");
                var adapter = SQLiteAdapter.From(fileName);
                Adapters.Add(adapter.SelectAppName(), adapter);
            }
        }

        public IEnumerable<LogMessage> AllLogsForApp(string appName)
        {
            if (!Adapters.ContainsKey(appName))
            {
                ScarifHub.Persistence.InsertInternalLog("SQLitePersistence:AllLogsForApp", "Error", $"No adapter found for app {appName}");
                return Enumerable.Empty<LogMessage>();
            }

            return Adapters[appName].SelectAllLogs();
        }

        public string AppNameFromUrl(string appUrl)
        {
            ScarifHub.Persistence.InsertInternalLog("SQLitePersistence:AppNameFromUrl", "Trace", $"Reading app name from {appUrl}");

            return Adapters.FirstOrDefault(a => a.Value.appUrl.Equals(appUrl)).Key;
        }

        public IEnumerable<ScarifApp> GetAllApps()
        {
            foreach (var adapter in Adapters)
            {
                if (adapter.Key is "Scarif")
                    continue;

                yield return new ScarifApp
                {
                    Name = adapter.Key,
                    Url = adapter.Value.appUrl
                };
            }
        }

        public bool InsertLog(LogMessage message)
        {
            var createdNewApp = false;
            if (!Adapters.ContainsKey(message.App))
            {
                // Received log from new app
                var appUrl = AppManager.AppUrlFromName(message.App);
                var newAdapter = SQLiteAdapter.From(appUrl);
                newAdapter.CreateNewApp(message.App);
                Adapters.Add(message.App, newAdapter);
                createdNewApp = true;
            }

            var adapter = Adapters.FirstOrDefault(a => a.Key.Equals(message.App)).Value;
            adapter.InsertLog(message);
            return createdNewApp;
        }

        public void InsertInternalLog(LogMessage message)
        {
            var adapter = Adapters["Scarif"];
            adapter.InsertLog(message);
        }

        public void InsertInternalLog(string component, string severity, string message)
        {
            var proto = new LogMessage
            {
                App = "Scarif",
                Component = component,
                Severity = severity,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message
            };
            InsertInternalLog(proto);
        }
    }
}
