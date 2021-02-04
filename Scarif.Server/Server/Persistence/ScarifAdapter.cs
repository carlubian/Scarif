using Scarif.Core.Model;
using Scarif.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scarif.Server.Server.Persistence
{
    public static class ScarifAdapter
    {
        public static void InsertApp(string appId, string appName)
        {
            var Scarif = new ScarifContext();

            // Only register app once, ignore subsequent requests
            if (Scarif.Apps.Any(a => a.AppId.Equals(appId)))
                return;

            // Insert new app
            Scarif.Apps.Add(new App
            {
                AppId = appId,
                AppName = appName
            });
            Scarif.SaveChanges();
        }

        public static void InsertLog(LogMessage logProto)
        {
            var Scarif = new ScarifContext();

            // Only add logs to registered apps
            if (!Scarif.Apps.Any(a => a.AppId.Equals(logProto.App)))
                return;

            // Insert log and properties
            var App = Scarif.Apps.Single(a => a.AppId.Equals(logProto.App));
            var Log = new Log
            {
                App = App,
                AppId = App.AppId,
                LogId = Guid.NewGuid(),
                Component = logProto.Component,
                Severity = logProto.Severity,
                Timestamp = logProto.Timestamp.ToDateTime(),
                Message = logProto.Message,
                Properties = new List<Property>()
            };
            foreach (var property in logProto.Properties)
                Log.Properties.Add(new Property
                {
                    Log = Log,
                    LogId = Log.LogId,
                    Key = property.Key,
                    Value = property.Value
                });
            App.Logs.Add(Log);

            Scarif.SaveChanges();
        }
    }
}
