# Scarif Log Server

Centralized, structured and dynamic log system.

## Introduction

TODO Fill this section

## Components

* Scarif.Core: Elements shared across multiple parts of the system, such as Protobuf definitions.
* Scarif.Server: ASP.NET Core WebAPI server that receives and stores log entries. Also includes a Blazor/WebAssembly client.
* Scarif.Source: Library used in apps that generate log data for Scarif.
