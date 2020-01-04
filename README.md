# Playing-with-Serilog

This small application is an example to use [Serilog](https://github.com/serilog/serilog) logging library in ASP.NET Core WebAPI.

Using Serilog you can avoid to dealing with the [Unit Testing with .NET Core ILogger](https://codeburst.io/unit-testing-with-net-core-ilogger-t-e8c16c503a80).

[Separate branch](https://github.com/19balazs86/Playing-with-Serilog/tree/netcoreapp2.2) with the .NET Core 2.2 version.

#### In this project you can find

- Create logger on the fly and create from file configuration.
- Create logger from [Consul](https://www.consul.io) configuration. Article about: [Dynamic ASP.NET Core configurations with Consul](https://www.c-sharpcorner.com/article/dynamic-asp-net-core-configurations-with-consul-kv).
- Use LoggingLevelSwitch, which helps you to change the minimum log level on the fly, without restart the server or change the configuration. [Dynamically changing the Serilog level](https://nblumhardt.com/2014/10/dynamically-changing-the-serilog-level).
- Calculate the execution time and display it in the log.
- Using the new [Request logging](https://github.com/serilog/serilog-aspnetcore#request-logging-300-) middleware feature from the [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore) 3.0.0 version.
  - [Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity) *(Andrew Lock)*
- Using [Serilog.Exceptions](https://github.com/RehanSaeed/Serilog.Exceptions) library to log exception details and custom properties that are not output by default.

#### Get Consul

- Docker
- [Consul/Download](https://www.consul.io/downloads.html). Run: consul agent -dev -client=0.0.0.0 | http://localhost:8500
