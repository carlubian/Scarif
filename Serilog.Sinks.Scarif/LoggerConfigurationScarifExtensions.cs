using Serilog.Configuration;

namespace Serilog.Sinks.Scarif
{
    public static class LoggerConfigurationScarifExtensions
    {
        public static LoggerConfiguration Scarif(
            this LoggerSinkConfiguration loggerConfiguration,
            string scarifEndpoint,
            string appName)
        {
            return loggerConfiguration.Sink(
                new ScarifSink(scarifEndpoint, appName));
        }
    }
}
