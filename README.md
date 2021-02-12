# Scarif Log Server

Centralized, structured and dynamic log system.

## Introduction

TODO Fill this section

## Requirements

Scarif has the following requirements to function:

* Server or app to deploy the Scarif Server. The tests have been made with an [Azure App Service](https://azure.microsoft.com/en-gb/services/app-service), but other web app providers should be compatible.
* SQL Database accessible from the server. The tests have been made with an [Azure Database for PostgreSQL](https://azure.microsoft.com/en-gb/services/postgresql), but other database providers should be compatible with minimal code changes.

## Configuration

Prior to starting the server, you will need to specify the connection parameters for the database.
To do so, create a <code>settings.conf</code> file in the same directory as the server executable,
and put the following content inside it:

```ini
[Postgres]
Server = <your-server>.postgres.database.azure.com
Database = <db-name>
Port = 5432
User = <user-name>
Password = <password>
```

Make sure to replace the placeholder values with correct parameters.

## Components

* Scarif.Core: Elements shared across multiple parts of the system, such as Protobuf definitions.
* Scarif.Server: ASP.NET Core WebAPI server that receives and stores log entries. Also includes a Blazor/WebAssembly client.
* Scarif.Source: Library used in apps that generate log data for Scarif.
* Serilog.Sinks.Scarif: Allows logs from Serilog to be written into a Scarif server.
