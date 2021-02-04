using Scarif.Source;
using Serilog.Core;
using Serilog.Events;
using System;

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

            Connect();
        }

        private void Connect()
        {
            Scarif = LogSource.Builder
                .UseSignalR(Endpoint)
                .SetAppName(AppName)
                .Build();
        }

        public void Emit(LogEvent logEvent)
        {
            /**
             * if (Scarif.IsConnected)
             * {
             *      switch (logEvent.Level)
             *      {
             *          ...
             *      }
             * }
             * else
             * {
             *      
             * }
             */
            throw new NotImplementedException();
        }
    }
}
