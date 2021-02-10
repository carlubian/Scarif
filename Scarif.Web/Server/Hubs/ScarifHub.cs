using Microsoft.AspNetCore.SignalR;
using Scarif.Core.Model;
using Scarif.Web.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Web.Server.Hubs
{
    public class ScarifHub : Hub
    {
        public void Test(string msg)
        {
            Console.WriteLine(msg);
        }

        public IEnumerable<App> GetApps()
        {
            var Scarif = new ScarifContext();
            return Scarif.Apps;
        }
    }
}
