using Google.Protobuf.WellKnownTypes;
using Microsoft.Data.Sqlite;
using Scarif.Protobuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Scarif.Source.Test")]
namespace Scarif.Server.Server.Core
{
    internal class SQLiteAdapter : IDisposable
    {
        private SqliteConnection SQLite;
        internal string appUrl;

        private SQLiteAdapter(string connStr, string appUrl)
        {
            SQLite = new SqliteConnection(connStr);
            SQLite.Open();
            this.appUrl = appUrl;
        }

        internal static SQLiteAdapter From(string file)
        {
            var query = $"Data Source = {Path.Combine("ScarifApps", $"{file}.scarif")}";
            Console.WriteLine($"Running SQLite from {query}");
            return new SQLiteAdapter(query, file);
        }

        public void Dispose()
        {
            SQLite.Close();
            SQLite.Dispose();
        }

        internal void CreateNewApp(string appName)
        {
            // Create the system metadata table
            var tblSystem = SQLite.CreateCommand();
            tblSystem.CommandText = "CREATE TABLE Scarif (Key VARCHAR(128) PRIMARY KEY, Value VARCHAR(256))";
            tblSystem.ExecuteNonQuery();

            var tblInsertApp = SQLite.CreateCommand();
            tblInsertApp.CommandText = $"INSERT INTO Scarif (Key, Value) VALUES ('AppName', '{appName}')";
            tblInsertApp.ExecuteNonQuery();

            // Create the logs table
            var tblLogs = SQLite.CreateCommand();
            tblLogs.CommandText = "CREATE TABLE Logs (App VARCHAR(256), Component VARCHAR(256), Severity VARCHAR(64), Timestamp TEXT, Message VARCHAR(4096))";
            tblLogs.ExecuteNonQuery();
        }

        internal string SelectAppName()
        {
            var tblSelect = SQLite.CreateCommand();
            tblSelect.CommandText = "SELECT Value FROM Scarif WHERE Key = 'AppName'";
            return tblSelect.ExecuteScalar().ToString();
        }

        internal IEnumerable<LogMessage> SelectAllLogs()
        {
            var tblSelect = SQLite.CreateCommand();
            tblSelect.CommandText = "SELECT App, Component, Severity, Timestamp, Message FROM Logs ORDER BY Timestamp DESC LIMIT 100";
            var reader = tblSelect.ExecuteReader();

            while(reader.Read())
            {
                var message = new LogMessage
                {
                    App = reader.GetString(0),
                    Component = reader.GetString(1),
                    Severity = reader.GetString(2),
                    Timestamp = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.ParseExact(reader.GetString(3), "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture), DateTimeKind.Utc)),
                    Message = reader.GetString(4)
                };
                yield return message;
            }
        }

        internal void InsertLog(LogMessage message)
        {
            var tblInsert = SQLite.CreateCommand();
            tblInsert.CommandText = $"INSERT INTO Logs (App, Component, Severity, Timestamp, Message) VALUES ('{message.App}', '{message.Component}', '{message.Severity}', '{message.Timestamp.ToDateTime():yyyy-MM-ddTHH:mm:ss.fff}', '{message.Message}')";
            tblInsert.ExecuteNonQuery();
        }
    }
}
