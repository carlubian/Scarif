using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Scarif.Core.Model;
using Scarif.Web.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scarif.Web.Server.Hubs
{
    public class ScarifHub : Hub
    {
        public void Test(string msg)
        {
            Console.WriteLine(msg);
        }

        public IEnumerable<App> GetApps()
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps;
        }

        public IEnumerable<Log> LogsForApp(string AppId)
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps
                .Include(a => a.Logs)
                .ThenInclude(l => l.Properties)
                .FirstOrDefault(a => a.AppId.Equals(AppId))?
                .Logs?
                .OrderByDescending(l => l.Timestamp)
                ?? Enumerable.Empty<Log>();
        }

        public string AppNameFromId(string AppId)
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps
                .FirstOrDefault(a => a.AppId.Equals(AppId))?
                .AppName ?? "Unknown app";
        }
    }
}
