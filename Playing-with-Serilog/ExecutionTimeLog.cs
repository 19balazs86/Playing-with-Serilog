using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace Playing_with_Serilog
{
  public class ExecutionTimeLog : IDisposable
  {
    protected readonly Stopwatch _stopwatch;
    protected readonly LogEventLevel _level;
    protected string _messageTemplate;

    public ExecutionTimeLog(string messageTemplate = "Process executed in {ElapsedMillisec} ms", LogEventLevel level = LogEventLevel.Debug)
    {
      _stopwatch       = new Stopwatch();
      _level           = level;
      _messageTemplate = messageTemplate;

      _stopwatch.Start();
    }

    public void Dispose()
    {
      _stopwatch.Stop();

      // If you miss the ElapsedMillisec from the template, LogContext.PushProperty put it into the Properties.
      using (LogContext.PushProperty("ElapsedMillisec", _stopwatch.ElapsedMilliseconds))
        Log.Write(_level, _messageTemplate, _stopwatch.ElapsedMilliseconds);
    }
  }
}
