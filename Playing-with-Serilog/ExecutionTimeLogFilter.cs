using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Playing_with_Serilog
{
  public class ExecutionTimeLogFilter : Attribute, IActionFilter, IFilterFactory
  {
    private readonly Stopwatch _stopwatch;

    private readonly ILogger<ExecutionTimeLogFilter> _logger;

    private readonly Action<ILogger, long, Exception> _definedLogger
      = LoggerMessage.Define<long>(
          logLevel: LogLevel.Trace,
          eventId: 1,
          formatString: "Action executed in {ElapsedMillisec} ms");

    public bool IsReusable => false;

    public ExecutionTimeLogFilter(ILogger<ExecutionTimeLogFilter> logger)
    {
      _logger    = logger;
      _stopwatch = new Stopwatch();
    }

    public void OnActionExecuting(ActionExecutingContext context)
      => _stopwatch.Start();

    public void OnActionExecuted(ActionExecutedContext context)
    {
      _stopwatch.Stop();

      _definedLogger(_logger, _stopwatch.ElapsedMilliseconds, context.Exception);
    }

    // Needs to create a new filter due to the Stopwatch.
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
      => serviceProvider.GetRequiredService<ExecutionTimeLogFilter>();
  }
}
