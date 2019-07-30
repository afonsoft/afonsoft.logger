using Microsoft.Extensions.Logging;
using System;

namespace LoggerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var log = new Afonsoft.Logger.AfonsoftLoggerProvider<Program>().CreateLogger();
            log.LogInformation("TESTE");
            Console.ReadKey();
        }
    }
}
