using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Playing_with_Serilog
{
  public class Startup
  {
    private LoggingLevelSwitch _loggingLevelSwitch;

    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      initLogger();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc(options =>
      {
        options.Filters.Add<ExecutionTimeLogFilter>(); // Add filter
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddSingleton(_loggingLevelSwitch);

      services.AddTransient<ExecutionTimeLogFilter>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      // Make sure to flush the log.
      appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

      app.UseMvc();
    }

    private void initLogger()
    {
      // --> Init: Logging level switch
      LogEventLevel defaultLogLevel = Enum.Parse<LogEventLevel>(Configuration["Serilog:MinimumLevel:Default"]);

      _loggingLevelSwitch = new LoggingLevelSwitch(defaultLogLevel);

      // --> Create: Logger from config file
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(Configuration)
        .MinimumLevel.ControlledBy(_loggingLevelSwitch)
        .Enrich.WithProperty("EnrichProperty", "value")
        .CreateLogger();

      // --> Create: Logger 
      //Log.Logger = new LoggerConfiguration()
      //  .MinimumLevel.ControlledBy(_loggingLevelSwitch)
      //  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
      //  .MinimumLevel.Override("System", LogEventLevel.Warning)
      //  .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Internal.WebHost", LogEventLevel.Information)
      //  .Enrich.FromLogContext()
      //  .Enrich.WithProperty("EnrichProperty", "value")
      //  .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
      //  .WriteTo.File(path: "log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, formatter: new Serilog.Formatting.Json.JsonFormatter())
      //  //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
      //  //{
      //  //  AutoRegisterTemplate        = true,
      //  //  AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
      //  //}) // Install-Package Serilog.Sinks.ElasticSearch
      //  .CreateLogger();
    }
  }
}
