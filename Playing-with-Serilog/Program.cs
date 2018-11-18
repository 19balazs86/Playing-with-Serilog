using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Playing_with_Serilog
{
  public class Program
  {
    public static void Main(string[] args)
    {
      IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json", optional: true)
        //.AddEnvironmentVariables() if you need
        .AddCommandLine(args)
        .Build();

      createWebHostBuilder(config).Build().Run();
    }

    private static IWebHostBuilder createWebHostBuilder(IConfiguration config) =>
      new WebHostBuilder()
        .UseConfiguration(config)
        .UseKestrel()
        .ConfigureAppConfiguration((hostContext, configBuilder) =>
        {
          configBuilder.AddJsonFile("appsettings.json", true);
          configBuilder.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
        })
        .UseStartup<Startup>()
        .UseSerilog();
  }
}
