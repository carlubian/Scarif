using Scarif.Core;
using Scarif.Core.Model;
using Scarif.Protobuf;
using Scarif.Server.Server.Core;
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

            // Get all app files and open as SQLite
            var appUrls = Directory.EnumerateFiles(logDir, "*.scarif");
            foreach (var appUrl in appUrls)
            {
                var fileName = new FileInfo(appUrl).Name.Replace(".scarif", "");
                Console.WriteLine($"Located app in {appUrl}: {fileName}");
                var adapter = SQLiteAdapter.From(fileName);
                Adapters.Add(adapter.SelectAppName(), adapter);
            }
        }

        public IEnumerable<LogMessage> AllLogsForApp(string appName)
        {
            if (!Adapters.ContainsKey(appName))
            {
                Console.WriteLine($"No adapter found for app {appName}");
                return Enumerable.Empty<LogMessage>();
            }

            Console.WriteLine($"Reading all logs from {appName}");
            return Adapters[appName].SelectAllLogs();
        }

        public string AppNameFromUrl(string appUrl)
        {
            Console.WriteLine($"Reading app name from {appUrl}");

            return Adapters.FirstOrDefault(a => a.Value.appUrl.Equals(appUrl)).Key;
        }

        public IEnumerable<ScarifApp> GetAllApps()
        {
            foreach (var adapter in Adapters)
            {
                yield return new ScarifApp
                {
                    Name = adapter.Key,
                    Url = adapter.Value.appUrl
                };
            }
        }

        public void InsertLog(LogMessage message)
        {
            var adapter = Adapters.FirstOrDefault(a => a.Key.Equals(message.App)).Value;
            adapter.InsertLog(message);
        }
    }
}
