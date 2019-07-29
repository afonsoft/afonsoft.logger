using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Afonsoft.Logger
{
    /// <summary>
    /// LoggerExtensions
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// builder.Services.AddSingleton<ILoggerProvider, LoggerProvider<T>>(p => new LoggerProvider<T>(typeof(T).ToString(), (categoryName, logLevel) => logLevel >= LogLevel.Debug));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggerProvider<T>>(p => new LoggerProvider<T>(typeof(T).ToString(), (categoryName, logLevel) => logLevel >= LogLevel.Debug));
            return builder;
        }

        /// <summary>
        /// builder.Services.AddSingleton<ILoggerProvider, LoggerProvider<T>>(p => new LoggerProvider<T>(typeof(T).ToString(), filter));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder, Func<string, LogLevel, bool> filter)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggerProvider<T>>(p => new LoggerProvider<T>(typeof(T).ToString(), filter));
            return builder;
        }

        /// <summary>
        /// loggerFactory.AddProvider(new LoggerProvider<T>((categoryName, logLevel) => logLevel >= LogLevel.Debug));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger<T>(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new LoggerProvider<T>((categoryName, logLevel) => logLevel >= LogLevel.Debug));
            return loggerFactory;
        }

        /// <summary>
        /// loggerFactory.AddProvider(new LoggerProvider<T>(filter));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger<T>(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> filter)
        {
            loggerFactory.AddProvider(new LoggerProvider<T>(filter));
            return loggerFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory, string categoryName)
        {
            loggerFactory.AddProvider(new LoggerProvider(categoryName, (category, logLevel) => logLevel >= LogLevel.Debug));
            return loggerFactory;
        }

        /// <summary>
        ///  loggerFactory.AddProvider(new LoggerProvider<ILogger>(categoryName, filter));
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory, string categoryName, Func<string, LogLevel, bool> filter)
        {
            loggerFactory.AddProvider(new LoggerProvider(categoryName, filter));
            return loggerFactory;
        }
    }
}
