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
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider<T>>();
            return builder;
        }

        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configure">Configure an instance of the <see cref="AfonsoftLoggerOptions" /> to set logging options</param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder, Action<AfonsoftLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAfonsoftLoggerProvider<T>();
            builder.Services.Configure(configure);
            return builder;
        }

        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, AfonsoftLoggerProvider>();
            return builder;
        }

        /// <summary>
        /// ILoggingBuilder AddAfonsoftLoggerProvider
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure">Configure an instance of the <see cref="AfonsoftLoggerOptions" /> to set logging options</param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider(this ILoggingBuilder builder, Action<AfonsoftLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddAfonsoftLoggerProvider();
            builder.Services.Configure(configure);
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
            loggerFactory.AddProvider(new AfonsoftLoggerProvider<T>());
            return loggerFactory;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loggerFactory"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger<T>(this ILoggerFactory loggerFactory, Func<AfonsoftLoggerOptions> configure)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider<T>(configure));
            return loggerFactory;
        }


        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider());
            return loggerFactory;
        }

        /// <summary>
        /// ILoggerFactory AddAfonsoftLogger
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ILoggerFactory AddAfonsoftLogger(this ILoggerFactory loggerFactory, Func<AfonsoftLoggerOptions> configure)
        {
            loggerFactory.AddProvider(new AfonsoftLoggerProvider(configure));
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
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging(this IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider();
            });

            return services;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging<T>(this IServiceCollection services, Action<AfonsoftLoggerOptions> configure)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider<T>(configure);
            });
            
            return services;
        }

        /// <summary>
        /// IServiceCollection AddAfonsoftLogging
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftLogging(this IServiceCollection services, Action<AfonsoftLoggerOptions> configure)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider(configure);
            });

            
            return services;
        }
    }
}
