using Serilog;
using Serilog.Sinks.RollingFileAlternate;

namespace rainfall_api.Common
{
    public static class StaticLogger
    {
        public static void EnsureInitialized()
        {
            if (Log.Logger is not Serilog.Core.Logger)
            {
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.RollingFileAlternate(".\\logs")
                    .WriteTo.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
            }
        }
    }
}
