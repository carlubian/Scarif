using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.SignalR.Client;
using Scarif.Core;
using Scarif.Protobuf;
using Scarif.Source.Builder;
using System;
using System.Threading.Tasks;

namespace Scarif.Source
{
    public class LogSource
    {
        private readonly string Endpoint;
        private readonly string App;

        private HubConnection SignalR;

        internal LogSource(string endpoint, string app)
        {
            Endpoint = endpoint;
            App = app;

            ConnectSignalR();
        }

        private void ConnectSignalR()
        {
            SignalR = new HubConnectionBuilder()
                .WithUrl($"{Endpoint}/scarif")
                .Build();
            SignalR.StartAsync().Wait();

            SignalR.InvokeAsync("ReportNewSource", App);
        }

        public static LogSourceBuilder Builder => new LogSourceBuilder();

        public LogSourceInstance ForComponent(string component) => new LogSourceInstance(this, component);

        internal void SendLog(string component, LogSeverity severity, string message)
        {
            var proto = new LogMessage
            {
                App = App,
                Component = component,
                Severity = severity.ToString(),
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = message
            };
            var base64 = proto.ToByteString().ToBase64();

            if (SignalR is null || SignalR.State != HubConnectionState.Connected)
                ConnectSignalR();

            Parallel.Invoke(() => SignalR.InvokeAsync("ReceiveLog", base64).Wait());
        }

        public void Trace(string component, string message) => SendLog(component, LogSeverity.Trace, message);

        public void Info(string component, string message) => SendLog(component, LogSeverity.Info, message);

        public void Warning(string component, string message) => SendLog(component, LogSeverity.Warning, message);

        public void Error(string component, string message) => SendLog(component, LogSeverity.Error, message);
    }
}
