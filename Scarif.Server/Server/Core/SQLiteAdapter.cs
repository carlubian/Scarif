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
        private readonly SqliteConnection SQLite;
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
            return new SQLiteAdapter(query, file);
        }

        internal static SQLiteAdapter CreateInternalAdapter()
        {
            var file = Path.Combine("ScarifApps", $"Internal.Logs.scarif");
            var query = $"Data Source = {file}";

            if (!File.Exists(file))
            {
                // Create new internal log file
                var sqlite = new SqliteConnection(query);
                sqlite.Open();

                // Create the system metadata table
                var tblSystem = sqlite.CreateCommand();
                tblSystem.CommandText = "CREATE TABLE Scarif (Key VARCHAR(128) PRIMARY KEY, Value VARCHAR(256))";
                tblSystem.ExecuteNonQuery();

                var tblInsertApp = sqlite.CreateCommand();
                tblInsertApp.CommandText = $"INSERT INTO Scarif (Key, Value) VALUES ('AppName', 'Scarif')";
                tblInsertApp.ExecuteNonQuery();

                // Create the logs table
                var tblLogs = sqlite.CreateCommand();
                tblLogs.CommandText = "CREATE TABLE Logs (App VARCHAR(256), Component VARCHAR(256), Severity VARCHAR(64), Timestamp TEXT, Message VARCHAR(4096))";
                tblLogs.ExecuteNonQuery();

                sqlite.Close();
            }

            return new SQLiteAdapter(query, "scarif");
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
            return tblSelect.ExecuteScalar().ToString() ?? "[Unknown App]";
        }

        internal IEnumerable<LogMessage> SelectAllLogs(bool[] severities)
        {
            // Modify command for the required severities
            var cmd = "SELECT App, Component, Severity, Timestamp, Message FROM Logs";
            if (severities.Any())
                cmd += " WHERE ";
            if (severities[0])
                cmd += "Severity = 'Trace' OR ";
            if (severities[1])
                cmd += "Severity = 'Info' OR ";
            if (severities[2])
                cmd += "Severity = 'Warning' OR ";
            if (severities[3])
                cmd += "Severity = 'Error' OR ";
            if (severities.Any())
                cmd = cmd.Remove(cmd.Length - 3);
            cmd += " ORDER BY Timestamp DESC LIMIT 100";

            Console.WriteLine(cmd);

            var tblSelect = SQLite.CreateCommand();
            tblSelect.CommandText = cmd;
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
