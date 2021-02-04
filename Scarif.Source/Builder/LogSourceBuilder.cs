using System;
using System.Collections.Generic;
using System.Text;

namespace Scarif.Source.Builder
{
    public class LogSourceBuilder
    {
        private string Endpoint = "localhost";
        private bool IsSignalR = true;
        private string App = "Default app";

        public LogSourceBuilder UseSignalR(string endpoint)
        {
            Endpoint = endpoint;
            IsSignalR = true;
            return this;
        }

        public LogSourceBuilder UseHttp(string endpoint)
        {
            Endpoint = endpoint;
            IsSignalR = false;
            return this;
        }

        public LogSourceBuilder SetAppName(string app)
        {
            App = app;
            return this;
        }

        public LogSource Build() => new LogSource(Endpoint, App, IsSignalR);
    }
}
