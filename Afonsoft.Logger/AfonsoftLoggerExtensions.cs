using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Afonsoft.Logger
{
    /// <summary>
    /// LoggerExtensions
    /// </summary>
    public static class AfonsoftLoggerExtensions
    {
        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider<T>>(p => new AfonsoftLoggerProvider<T>(typeof(T).ToString(), (categoryName, logLevel) => logLevel >= LogLevel.Debug));
            builder.AddProvider(new AfonsoftLoggerProvider<T>(typeof(T).ToString(), (name, logLevel) => logLevel >= LogLevel.Debug));
            builder.Services.AddSingleton<ILogger<T>>(new AfonsoftLoggerProvider<T>().CreateLogger<T>());
            builder.Services.AddSingleton<ILogger>(new AfonsoftLoggerProvider<T>().CreateLogger());
            return builder;
        }
/// <summary>
/// 
/// </summary>
/// <param name="builder"></param>
/// <param name="categoryName"></param>
/// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider(this ILoggingBuilder builder, string categoryName)
        {
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider>(p => new AfonsoftLoggerProvider(categoryName, (category, logLevel) => logLevel >= LogLevel.Debug));
            builder.AddProvider(new AfonsoftLoggerProvider(categoryName, (name, logLevel) => logLevel >= LogLevel.Debug));
            builder.Services.AddSingleton<ILogger>(new AfonsoftLoggerProvider(categoryName).CreateLogger());
            return builder;
        }

        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="categoryName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider(this ILoggingBuilder builder, string categoryName, Func<string, LogLevel, bool> filter)
        {
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider>(p => new AfonsoftLoggerProvider(categoryName, filter));
            builder.AddProvider(new AfonsoftLoggerProvider(categoryName, filter));
            builder.Services.AddSingleton<ILogger>(new AfonsoftLoggerProvider(categoryName, filter).CreateLogger());
            return builder;
        }

        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder, Func<string, LogLevel, bool> filter)
        {
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider<T>>(p => new AfonsoftLoggerProvider<T>(typeof(T).ToString(), filter));
            builder.AddProvider(new AfonsoftLoggerProvider<T>(typeof(T).ToString(), filter));
            builder.Services.AddSingleton<ILogger<T>>(new AfonsoftLoggerProvider<T>().CreateLogger<T>(filter));
            builder.Services.AddSingleton<ILogger>(new AfonsoftLoggerProvider<T>().CreateLogger(typeof(T).ToString(), filter));
            return builder;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger<T>(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider<T>((categoryName, logLevel) => logLevel >= LogLevel.Debug));
            return loggerFactory;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger<T>(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> filter)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider<T>(filter));
            return loggerFactory;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory, string categoryName)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider(categoryName, (category, logLevel) => logLevel >= LogLevel.Debug));
            return loggerFactory;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory, string categoryName, Func<string, LogLevel, bool> filter)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider(categoryName, filter));
            return loggerFactory;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging<T>(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider<T>();
            });
                       
            return services;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <param name="services"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging(this IServiceCollection services, string categoryName)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider(categoryName);
            });

            return services;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging<T>(this IServiceCollection services, Func<string, LogLevel, bool> filter)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider<T>(filter);
            });
            
            return services;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <param name="services"></param>
        /// <param name="categoryName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging(this IServiceCollection services, string categoryName, Func<string, LogLevel, bool> filter)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider(categoryName, filter);
            });

            
            return services;
        }
    }
}
