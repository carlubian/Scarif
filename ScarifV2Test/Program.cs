using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Scarif.Protobuf;
using Serilog;
using Serilog.Sinks.Scarif;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ScarifV2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Scarif("http://localhost:8585", "Scarif Sink Test")
                .CreateLogger();

            Log.Information("[{Component}] has information message: {Message}. RandomVar is {RandomVar}", "Program:Main", "Test message goes here", true);

            //var Http = new HttpClient();

            //var protoLog = new LogMessage
            //{
            //    App = "scarifv2test",
            //    Component = "Program:Main",
            //    Severity = "info",
            //    Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
            //    Message = "Test message"
            //};
            //protoLog.Properties.AddRange(new LogProperty[]
            //{
            //    new LogProperty
            //    {
            //        Key = "user",
            //        Value = "Paco"
            //    },
            //    new LogProperty
            //    {
            //        Key = "account",
            //        Value = "2"
            //    }
            //});

            //var protoBase64 = protoLog.ToByteString().ToBase64();
            //var content = new Dictionary<string, string>()
            //{
            //    { "log", protoBase64 }
            //};

            //// Register app
            //var newApp = new Dictionary<string, string>()
            //{
            //    { "appName", "Scarif v2 Test" },
            //    { "appUrl", "scarifv2test" }
            //};
            //Http.PutAsync("http://localhost:8585/api", new FormUrlEncodedContent(newApp)).Wait();

            //// Send log
            //Http.PostAsync("http://localhost:8585/api", new FormUrlEncodedContent(content)).Wait();
        }
    }
}
