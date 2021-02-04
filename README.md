# Scarif Log Server

Centralized, structured and dynamic log system.

## Introduction

TODO Fill this section

## Requirements

In order to deploy V2 of Scarif without modifications, you will need to have an instance of 
[https://azure.microsoft.com/en-gb/services/postgresql](Azure Database for PostgreSQL) or other compatible and accesible SQL database. This database will be used by Scarif to store all incoming logs.

## Components

* Scarif.Core: Elements shared across multiple parts of the system, such as Protobuf definitions.
* Scarif.Server: ASP.NET Core WebAPI server that receives and stores log entries. Also includes a Blazor/WebAssembly client.
* Scarif.Source: Library used in apps that generate log data for Scarif.
* Serilog.Sinks.Scarif: Allows logs from Serilog to be written into a Scarif server.
