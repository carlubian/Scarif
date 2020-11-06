using System;
using System.Collections.Generic;
using System.Text;

namespace Scarif.Source
{
    public class LogSourceInstance
    {
        private readonly LogSource Source;
        private readonly string Component;

        internal LogSourceInstance(LogSource source, string component)
        {
            Source = source;
            Component = component;
        }

        public void Trace(string message) => Source.Trace(Component, message);

        public void Info(string message) => Source.Info(Component, message);

        public void Warning(string message) => Source.Warning(Component, message);

        public void Error(string message) => Source.Error(Component, message);
    }
}
