using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog.Events;
using System.Collections.Generic;

namespace Playing_with_Serilog.Model
{
  public class GetLogLevelInfo
  {
    [JsonConverter(typeof(StringEnumConverter))]
    public LogEventLevel CurrentLogLevel { get; set; }

    public IEnumerable<string> LogLevels { get; set; }
  }
}
