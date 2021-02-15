using Google.Protobuf.WellKnownTypes;
using Scarif.Core;
using Scarif.Protobuf;
using Scarif.Source;
using Scarif.Source.Builder;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Scarif
{
    public class ScarifSink : ILogEventSink
    {
        private string Endpoint;
        private string AppName;
        private LogSource Scarif;

        public ScarifSink(string endpoint, string appName)
        {
            Endpoint = endpoint;
            AppName = appName;
            Scarif = new LogSourceBuilder()
                .UseHttp(Endpoint)
                .SetAppName(AppName)
                .Build();
        }

        public void Emit(LogEvent logEvent)
        {
            var Log = new LogMessage
            {
                App = AppManager.AppUrlFromName(AppName),
                Component = logEvent.Properties.ContainsKey("Component") ? logEvent.Properties["Component"].ToString() : "Unknown",
                Severity = logEvent.Level.ToString(),
                Timestamp = Timestamp.FromDateTime(logEvent.Timestamp.UtcDateTime),
                Message = logEvent.RenderMessage()
            };
            foreach (var prop in logEvent.Properties)
                Log.Properties.Add(new LogProperty
                {
                    Key = prop.Key,
                    Value = prop.Value.ToString()
                });

            Scarif.SendLog(Log);
        }
    }
}
