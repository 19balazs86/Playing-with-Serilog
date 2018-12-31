# Playing-with-Serilog

This small application is an example to **use [Serilog](https://github.com/serilog/serilog "Serilog") logging library** in ASP.NET Core WebAPI.

Github repository for [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore "Serilog.AspNetCore")

In this project you can find:
- Create logger on the fly and create from configuration.
- Use LoggingLevelSwitch, which helps you to change the minimum log level on the fly, without restart the server or change the configuration.
- Calculate the execution time and display it in the log.
- Startup file ConfigureServices: the default AddMvc extension method is replaced by **AddMvcCore**. You can save some resources by skipping include the razor functionality (usually, you do not use that in WebAPI). [Difference between AddMvc and AddMvcCore](https://offering.solutions/blog/articles/2017/02/07/difference-between-addmvc-addmvcore "Difference between AddMvc and AddMvcCore").