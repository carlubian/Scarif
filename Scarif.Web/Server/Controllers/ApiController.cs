using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Scarif.Core.Model;
using Scarif.Protobuf;
using Scarif.Web.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Web.Server.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ApiController
    {
        [HttpGet]
        public IEnumerable<App> GetAllApps()
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps;
        }

        [HttpPut]
        public void PutNewApp([FromForm]string appName, [FromForm]string appUrl)
        {
            var Scarif = new ScarifContext();

            // Only register new apps
            if (Scarif.Apps.Any(a => a.AppId.Equals(appUrl)))
                return;

            Scarif.Apps.Add(new App
            {
                AppId = appUrl,
                AppName = appName
            });
            Scarif.SaveChanges();
        }

        [HttpPost]
        public void PostNewLog([FromForm]string log)
        {
            var Scarif = new ScarifContext();
            var protoLog = LogMessage.Parser.ParseFrom(ByteString.FromBase64(log));

            // Requires a registered app
            var app = Scarif.Apps.SingleOrDefault(a => a.AppId.Equals(protoLog.App));
            if (app is null)
                return;

            var modelLog = new Log
            {
                App = app,
                Component = protoLog.Component,
                Severity = protoLog.Severity,
                Message = protoLog.Message,
                Timestamp = protoLog.Timestamp.ToDateTime(),
                Properties = new List<Property>()
            };

            Scarif.SaveChanges();

            foreach (var prop in protoLog.Properties)
                modelLog.Properties.Add(new Property
                {
                    Key = prop.Key,
                    Value = prop.Value
                });

            // Edge case for first log
            if (app.Logs is null)
                app.Logs = new List<Log>();

            app.Logs.Add(modelLog);
            Scarif.SaveChanges();
        }
    }
}
