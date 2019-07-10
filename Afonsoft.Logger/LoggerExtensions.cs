using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Afonsoft.Logger
{
    /// <summary>
    /// LoggerExtensions
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// AddProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddLoggerProvider<T>(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggerProvider<T>>(p => new LoggerProvider<T>(typeof(T).ToString(), (categoryName, logLevel) => logLevel >= LogLevel.Debug));
            return builder;
        }
    }
}