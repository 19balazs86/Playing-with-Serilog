using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playing_with_Serilog.Model;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Playing_with_Serilog.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class AdminController : ControllerBase
  {
    private readonly LoggingLevelSwitch _loggingLevelSwitch;

    public AdminController(LoggingLevelSwitch loggingLevelSwitch)
    {
      _loggingLevelSwitch = loggingLevelSwitch;

      // {SourceContext} property in the outputTemplate, can be populated with the class name.
      //Log.ForContext<AdminController>().Debug("Constructor: AdminController");
    }

    [HttpGet("min-loglevel")]
    public ActionResult<GetLogLevelInfo> GetMinimumLogLevel()
    {
      Log.Information("Get me the minimum log level");

      return new GetLogLevelInfo
      {
        CurrentLogLevel = _loggingLevelSwitch.MinimumLevel,
        LogLevels       = Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>().Select(x => x.ToString()).ToArray()
      };
    }

    [HttpPost("min-loglevel")]
    public void SetMinimumLogLevel(LogEventLevel logLevel)
    {
      Log.Information("Set the minimum log level to: {level}", logLevel);

      _loggingLevelSwitch.MinimumLevel = logLevel;
    }

    [HttpGet("test-execution-time")]
    public async Task TestExecutionTime()
    {
      using (ExecutionTimeLog eth1 = new ExecutionTimeLog("Task #1"))
      {
        await Task.Delay(300); // Do something

        using (ExecutionTimeLog eth2 = new ExecutionTimeLog("Task #2 executed in {ElapsedMillisec} ms"))
        {
          await Task.Delay(200); // Do something
        }
      }
    }
  }
}