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
    public static bool UsingConsul => false;

    private LoggingLevelSwitch _loggingLevelSwitch;

    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      // --> Init: Logger
      initLogger();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvcCore(options =>
      {
        options.Filters.Add(new ExecutionTimeLogActionFilter()); // Add filter
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
      //.AddApiExplorer()
      //.AddAuthorization()
      //.AddDataAnnotations()
      .AddJsonFormatters();
      //.AddCors();

      //services.AddMvc(options => {
      //  options.Filters.Add(new ExecutionTimeLogActionFilter()); // Add filter
      //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
      // --> Init: Logging level switch
      LogEventLevel defaultLogLevel = Enum.Parse<LogEventLevel>(Configuration["Serilog:MinimumLevel:Default"]);

      _loggingLevelSwitch = new LoggingLevelSwitch(defaultLogLevel);

      // --> Create: Logger from config file
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(Configuration)
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
