using Microsoft.AspNetCore.SignalR;
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
    }
}
