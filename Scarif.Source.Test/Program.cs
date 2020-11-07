using System;

namespace Scarif.Source.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var logSource = LogSource.Builder
                .UseSignalR("http://localhost:8585")
                .SetAppName("Test")
                .Build();

            logSource.Info("SourceTest:Program:Main", "Info message from source test.");

            //var sqlite = SQLiteAdapter.From("test.scarif");
            //Console.WriteLine(sqlite.SelectAppName());
            //var logs = sqlite.SelectAllLogs();
            //foreach (var log in logs)
            //    Console.WriteLine(log.Message);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
