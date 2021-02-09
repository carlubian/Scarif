using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Scarif.Protobuf;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ScarifV2Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var Http = new HttpClient();

            var protoLog = new LogMessage
            {
                App = "scarifv2test",
                Component = "Program:Main",
                Severity = "info",
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Message = "Test message"
            };
            protoLog.Properties.AddRange(new LogProperty[]
            {
                new LogProperty
                {
                    Key = "user",
                    Value = "Paco"
                },
                new LogProperty
                {
                    Key = "account",
                    Value = "2"
                }
            });

            var protoBase64 = protoLog.ToByteString().ToBase64();
            var content = new Dictionary<string, string>()
            {
                { "log", protoBase64 }
            };

            // Register app
            var newApp = new Dictionary<string, string>()
            {
                { "appName", "Scarif v2 Test" },
                { "appUrl", "scarifv2test" }
            };
            Http.PutAsync("http://localhost:8585/api", new FormUrlEncodedContent(newApp)).Wait();

            // Send log
            Http.PostAsync("http://localhost:8585/api", new FormUrlEncodedContent(content)).Wait();
        }
    }
}
