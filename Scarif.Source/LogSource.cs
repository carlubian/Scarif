using Google.Protobuf;
using Microsoft.AspNetCore.SignalR.Client;
using Scarif.Core;
using Scarif.Protobuf;
using Scarif.Source.Builder;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scarif.Source
{
    public class LogSource
    {
        private readonly string Endpoint;
        private readonly string AppName;

        private HubConnection SignalR;

        /// <summary>
        /// Determines whether the Log Source is connected to
        /// a valid endpoint. Only valid for SignalR sources.
        /// </summary>
        public bool IsConnected => SignalR is not null && SignalR.State is HubConnectionState.Connected;
        /// <summary>
        /// Determines whether this Log Source uses SignalR to
        /// connect to the Scarif server.
        /// </summary>
        public readonly bool IsSignalR;

#pragma warning disable CS8618 // SignalR will be initialized by ConnectSignalR()
        internal LogSource(string endpoint, string app, bool isSignalR)
#pragma warning restore CS8618 // 
        {
            Endpoint = endpoint;
            AppName = app;
            IsSignalR = isSignalR;

            if (IsSignalR)
                ConnectSignalR();
        }

        private void ConnectSignalR()
        {
            SignalR = new HubConnectionBuilder()
                .WithUrl($"{Endpoint}/scarifhub")
                .Build();
            SignalR.StartAsync().Wait();

            SignalR.InvokeAsync("ReportNewSource", AppName);
        }

        public static LogSourceBuilder Builder => new LogSourceBuilder();

        public void SendLog(LogMessage message)
        {
            var base64 = message.ToByteString().ToBase64();

            DoRegisterApp();
            DoSendLog(base64);
        }

        private void DoSendLog(string base64)
        {
            if (IsSignalR && !IsConnected)
                ConnectSignalR();

            if (IsSignalR)
                Parallel.Invoke(() => SignalR.InvokeAsync("ReceiveLog", base64).Wait());
            else
            {
                // Use standard HTTP request
                var Http = new HttpClient();

                // Make POST request to endpoint
                // with base64 in form data.
                var content = new Dictionary<string, string>()
                {
                    { "log", base64 }
                };
                Http.PostAsync($"{Endpoint}/api", new FormUrlEncodedContent(content)).Wait();
            }
        }

        private void DoRegisterApp()
        {
            var Http = new HttpClient();
            var newApp = new Dictionary<string, string>()
            {
                { "appName", AppName },
                { "appUrl", AppManager.AppUrlFromName(AppName) }
            };
            Http.PutAsync($"{Endpoint}/api", new FormUrlEncodedContent(newApp)).Wait();
        }
    }
}
