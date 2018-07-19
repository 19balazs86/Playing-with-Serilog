using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Playing_with_Serilog
{
  public class ExecutionTimeLogActionFilter : Attribute, IActionFilter, IFilterFactory
  {
    protected readonly Stopwatch _stopwatch;
    protected readonly LogEventLevel _level;
    protected string _messageTemplate;

    public ExecutionTimeLogActionFilter(LogEventLevel level = LogEventLevel.Verbose)
    {
      _messageTemplate = "Action executed in {ElapsedMillisec} ms";
      _stopwatch       = new Stopwatch();
      _level           = level;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
      _stopwatch.Start();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
      _stopwatch.Stop();

      // If you miss the ElapsedMillisec from the template, LogContext.PushProperty put it into the Properties.
      using (LogContext.PushProperty("ElapsedMillisec", _stopwatch.ElapsedMilliseconds))
        Log.Write(_level, _messageTemplate, _stopwatch.ElapsedMilliseconds);
    }

    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
      // If you add this filter in the ConfigureServices method, you need to create a new one.
      return new ExecutionTimeLogActionFilter();
    }
  }
}
