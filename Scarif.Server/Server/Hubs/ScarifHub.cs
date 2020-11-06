﻿using Microsoft.AspNetCore.SignalR;
using Scarif.Core.Model;
using Scarif.Server.Server.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Hubs
{
    public class ScarifHub : Hub
    {
        private IPersistence Persistence = new TestPersistence();

        private static IDictionary<string, string> OnlineSources = new Dictionary<string, string>();

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (OnlineSources.ContainsKey(Context.ConnectionId))
                OnlineSources.Remove(Context.ConnectionId);

            Clients.All.SendAsync("UpdateOnlineSources", OnlineSources);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Called from the client to get the list of apps.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScarifApp> GetAllApps()
        {
            return Persistence.GetAllApps();
        }

        /// <summary>
        /// Called from the client to convert AppUri to AppName
        /// </summary>
        /// <param name="appUrl"></param>
        /// <returns></returns>
        public string AppNameFromUrl(string appUrl)
        {
            return Persistence.AppNameFromUrl(appUrl);
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
        /// Called from log sources when they connect to report
        /// that they are online.
        /// </summary>
        /// <param name="appName"></param>
        public void ReportNewSource(string appName)
        {
            Console.WriteLine($"Connected source {Context.ConnectionId} for App {appName}");
            OnlineSources.Add(Context.ConnectionId, appName);
            Clients.All.SendAsync("UpdateOnlineSources", OnlineSources);
        }

        /// <summary>
        /// Called from log sources to send data to the server.
        /// Note: Data is encoded in Protobuf-Base64.
        /// </summary>
        /// <param name="logBase64"></param>
        public void ReceiveLog(string logBase64)
        {
            Console.WriteLine($"Received data: {logBase64}");
        }
    }
}