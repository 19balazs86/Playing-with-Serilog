# Playing-with-Serilog

This small application is an example to **use [Serilog](https://github.com/serilog/serilog "Serilog") logging library** in ASP.NET Core WebAPI.

Github repository for [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore "Serilog.AspNetCore")

**In this project you can find:**
- Create logger on the fly and create from file configuration.
- Create logger from [Consul](https://www.consul.io "Consul") configuration. Article about: [Dynamic ASP.NET Core configurations with Consul](https://www.c-sharpcorner.com/article/dynamic-asp-net-core-configurations-with-consul-kv "Dynamic ASP.NET Core configurations with Consul").
- Use LoggingLevelSwitch, which helps you to change the minimum log level on the fly, without restart the server or change the configuration. [Dynamically changing the Serilog level](https://nblumhardt.com/2014/10/dynamically-changing-the-serilog-level "Dynamically changing the Serilog level").
- Calculate the execution time and display it in the log.
- Startup file ConfigureServices: the default AddMvc extension method is replaced by **AddMvcCore**. You can save some resources by skipping include the razor functionality (usually, you do not use that in WebAPI). [Difference between AddMvc and AddMvcCore](https://offering.solutions/blog/articles/2017/02/07/difference-between-addmvc-addmvcore "Difference between AddMvc and AddMvcCore"). Other one: [Replace AddMvc with AddMvcCore](https://blog.codingmilitia.com/2019/01/26/aspnet-012-from-zero-to-overkill-move-to-a-web-api "Replace AddMvc with AddMvcCore").

**Get Consul:**
- Docker
- [Download](https://www.consul.io/downloads.html "Download"). Run: consul agent -dev -client=0.0.0.0
