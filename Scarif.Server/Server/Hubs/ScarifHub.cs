using Google.Protobuf;
using Microsoft.AspNetCore.SignalR;
using Scarif.Core.Model;
using Scarif.Protobuf;
using Scarif.Server.Server.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Hubs
{
    public class ScarifHub : Hub
    {
        private static readonly IDictionary<string, string> OnlineSources = new Dictionary<string, string>();

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (OnlineSources.ContainsKey(Context.ConnectionId))
            {
                //Persistence.InsertInternalLog("ScarifHub:OnDisconnectedAsync", "Info", $"Disconnected source {Context.ConnectionId} for App {OnlineSources[Context.ConnectionId]}");
                OnlineSources.Remove(Context.ConnectionId);
            }
            else
            {
                //Persistence.InsertInternalLog("ScarifHub:OnDisconnectedAsync", "Warning", $"Log source {Context.ConnectionId} disconnected wothout a registered app.");
            }

            Clients.All.SendAsync("UpdateOnlineSources", OnlineSources);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Called from the client to get the list of apps.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<App> GetAllApps()
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps;
        }

        /// <summary>
        /// Called from the client to request a manual update
        /// of the connected log sources.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> RequestOnlineSources()
        {
            return OnlineSources;
        }

        /// <summary>
        /// Called from the client to receive updated log
        /// messages for a specific app.
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public IEnumerable<LogMessage> RequestAppLogs(string appName, bool[] severities, string? componentFilter)
        {
            return Enumerable.Empty<LogMessage>();// Persistence.AllLogsForApp(appName, severities, componentFilter);
        }

        /// <summary>
        /// Called from log sources when they connect to report
        /// that they are online.
        /// </summary>
        /// <param name="appUrl"></param>
        public void ReportNewSource(string appUrl)
        {
            //Persistence.InsertInternalLog("ScarifHub:ReportNewSource", "Trace", $"Connected source {Context.ConnectionId} for App {appUrl}");
            OnlineSources.Add(Context.ConnectionId, appUrl);
            Clients.All.SendAsync("UpdateOnlineSources", OnlineSources);
        }

        /// <summary>
        /// Called form log sources to register a new app
        /// in the server. Can be called multiple times without
        /// duplicating apps.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appName"></param>
        public void ReceiveApp(string appId, string appName)
        {
            ScarifAdapter.InsertApp(appId, appName);
        }

        /// <summary>
        /// Called from log sources to send data to the server.
        /// Note: Data is encoded in Protobuf-Base64.
        /// </summary>
        /// <param name="logBase64"></param>
        public void ReceiveLog(string logBase64)
        {
            var message = LogMessage.Parser.ParseFrom(ByteString.FromBase64(logBase64));
            ScarifAdapter.InsertLog(message);

            //if (Persistence.InsertLog(message))
            //    Clients.All.SendAsync("NotifyNewAppCreated");
            Clients.All.SendAsync("NotifyIncomingLog", message.App);
        }
    }
}
