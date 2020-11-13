using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scarif.Server.Client.Core
{
    public delegate void UpdateEventHandler();

    public class UpdateService
    {
        public IDictionary<string, int> ForApp = new Dictionary<string, int>();
        public string? ActiveApp;
        public event UpdateEventHandler? Update;

        public void ReceiveLog(string app)
        {
            if (app.Equals(ActiveApp))
                return;

            if (ForApp.ContainsKey(app))
                ForApp[app] += 1;
            else
                ForApp.Add(app, 1);

            Update?.Invoke();
        }

        public void Clear(string? app)
        {
            if (app is not null && ForApp.ContainsKey(app))
                ForApp.Remove(app);

            Update?.Invoke();
        }
    }
}
