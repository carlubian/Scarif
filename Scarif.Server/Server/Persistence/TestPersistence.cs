﻿using Scarif.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Server.Persistence
{
    public class TestPersistence : IPersistence
    {
        public IEnumerable<ScarifApp> GetAllApps()
        {
            yield return new ScarifApp
            {
                Name = "SARAH",
                Url = "sarah"
            };
            yield return new ScarifApp
            {
                Name = "TimeChef",
                Url = "timechef"
            };
            yield return new ScarifApp
            {
                Name = "Agenda 2.0",
                Url = "agenda2"
            };
        }

        public string AppNameFromUrl(string appUrl)
        {
            switch (appUrl)
            {
                case "sarah":
                    return "SARAH";
                case "timechef":
                    return "TimeChef";
                case "agenda2":
                    return "Agenda 2.0";
                default:
                    return "[Unknown]";
            }
        }
    }
}
