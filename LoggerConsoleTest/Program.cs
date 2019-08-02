using Microsoft.Extensions.Logging;
using System;

namespace LoggerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var log = new Afonsoft.Logger.AfonsoftLoggerProvider<Program>(c=> c.Extension="aft").CreateLogger();
            log.LogInformation("LogInformation");
            log.LogError("LogError");
            log.LogDebug("LogDebug");
            log.LogWarning("LogWarning");
            log.LogTrace("LogTrace");
            log.LogCritical("LogCritical");
            Console.ReadKey();
        }
    }
}
