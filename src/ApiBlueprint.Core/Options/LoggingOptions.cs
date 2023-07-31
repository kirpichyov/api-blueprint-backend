using Serilog.Events;

namespace ApiBlueprint.Core.Options;

public sealed class LoggingOptions
{
    public LogEventLevel ConsoleLogLevel { get; init; }
}