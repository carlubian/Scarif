using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.SignalR.Client;
using Scarif.Core.Model;
using Scarif.Protobuf;
using Scarif.Source.Builder;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scarif.Source
{
    public class LogSource
    {
        private readonly string Endpoint;
        private readonly string App;

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
            App = app;
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

            SignalR.InvokeAsync("ReportNewSource", App);
        }

        public static LogSourceBuilder Builder => new LogSourceBuilder();

        public void SendLog(Log message)
        {
            var proto = new LogMessage
            {
                App = App,
                Component = message.Component,
                Severity = message.Severity,
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message.Message
            };
            foreach (var prop in message.Properties)
                proto.Properties.Add(new LogProperty
                {
                    Key = prop.Key,
                    Value = prop.Value
                });

            var base64 = proto.ToByteString().ToBase64();

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

                // TODO: Make POST request to endpoint
                // with base64 in form data.
            }
        }
    }
}
