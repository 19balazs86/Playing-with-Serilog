using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Winton.Extensions.Configuration.Consul;

namespace Playing_with_Serilog
{
  public class Program
  {
    public static bool UsingConsul => false;

    public static void Main(string[] args)
    {
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
      {
        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json", optional: true)
        //.AddEnvironmentVariables() if you need
        .AddCommandLine(args)
        .Build();

        createWebHostBuilder(config, cancellationTokenSource.Token).Build().Run();
      }
    }

    private static IWebHostBuilder createWebHostBuilder(IConfiguration config, CancellationToken cancellationToken)
    {
      return new WebHostBuilder()
        .UseConfiguration(config)
        .UseKestrel()
        .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
          string environmentName = hostContext.HostingEnvironment.EnvironmentName;

          if (UsingConsul)
          {
            configBuilder
              .AddConsul($"App1/appsettings.{environmentName}.json", cancellationToken,
                options =>
                {
                  // You won't get any exceptions, if it is optional and ignore exception.

                  options.ConsulConfigurationOptions = cco => cco.Address = new Uri("http://localhost:8500");
                  options.Optional = false;
                  options.ReloadOnChange = false;
                  options.OnLoadException = exceptionContext => exceptionContext.Ignore = false;
                });
          }
          else
          {
            configBuilder.AddJsonFile("appsettings.json", true);
            configBuilder.AddJsonFile($"appsettings.{environmentName}.json", true);
          }
        })
        .UseStartup<Startup>()
        .UseSerilog();
    }
  }
}
