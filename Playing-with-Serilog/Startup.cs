using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Playing_with_Serilog
{
  public class Startup
  {
    private LoggingLevelSwitch _loggingLevelSwitch;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      // --> Init: Logger
      initLogger();
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc(options => {
        options.Filters.Add(new ExecutionTimeLogActionFilter()); // Add filter
      }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.AddSingleton(_loggingLevelSwitch);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }

    private void initLogger()
    {
      // --> Read: Log configuration
      IConfiguration logConfig = new ConfigurationBuilder()
        .AddJsonFile("LogSettings.json")
        .Build();

      // --> Init: Logging level switch
      LogEventLevel defaultLogEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), logConfig["Serilog:MinimumLevel:Default"]);
      _loggingLevelSwitch = new LoggingLevelSwitch(defaultLogEventLevel);

      // --> Create: Logger from config file
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(logConfig)
        .MinimumLevel.ControlledBy(_loggingLevelSwitch)
        .CreateLogger();

      //// --> Create: Logger 
      //Log.Logger = new LoggerConfiguration()
      //  .MinimumLevel.ControlledBy(_loggingLevelSwitch)
      //  .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
      //  .Enrich.FromLogContext()
      //  .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
      //  .CreateLogger();
    }
  }
}
