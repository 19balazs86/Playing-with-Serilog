using System.Collections.Generic;
using Serilog.Events;

namespace Playing_with_Serilog.Model
{
  public class GetLogLevelInfo
  {
    public LogEventLevel CurrentLogLevel { get; set; }

    public IEnumerable<string> LogLevels { get; set; }
  }
}
