using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playing_with_Serilog.Model;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Playing_with_Serilog.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class AdminController : ControllerBase
  {
    private readonly LoggingLevelSwitch _loggingLevelSwitch;
    private readonly IDiagnosticContext _diagnosticContext;

    public AdminController(LoggingLevelSwitch loggingLevelSwitch, IDiagnosticContext diagnosticContext)
    {
      _loggingLevelSwitch = loggingLevelSwitch;
      _diagnosticContext  = diagnosticContext;

      // {SourceContext} property in the outputTemplate, can be populated with the class name.
      //Log.ForContext<AdminController>().Debug("Constructor: AdminController");
    }

    [HttpGet("min-loglevel")]
    public ActionResult<GetLogLevelInfo> GetMinimumLogLevel()
    {
      Log.Information("Get the minimum log level");

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

    [HttpGet("test-diagnostic-context/{id}")]
    public void TestDiagnosticContext(int id)
    {
      // Dynamically add properties during request processing.
      // TestId will appear in the log created by the app.UseSerilogRequestLogging().
      _diagnosticContext.Set("TestId", id);

      using (LogContext.PushProperty("TestId", id))
        Log.Information("LogContext.PushProperty dynamically add the TestId property this log.");
    }

    [HttpGet("test-exception-details")]
    public void TestExceptionDetails()
    {
      var innerException = new Exception("Test ExceptionDetails (InnerException)");
      innerException.Data.Add("MyKeyInner1", "MyValueInner1");
      innerException.Data.Add("MyKeyInner2", new { Prop1 = "PropertyInner1", Prop2 = 10 });

      var exception = new Exception("Test ExceptionDetails", innerException);
      exception.Data.Add("MyKey1", "MyValue1");
      exception.Data.Add("MyKey2", new { Prop1 = "Property1", Prop2 = 20 });

      Log.Error(exception, "Just a test for ExceptionDetails.");
    }
  }
}