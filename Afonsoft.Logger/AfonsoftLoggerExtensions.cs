using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Afonsoft.Logger.Logger<>));
            builder.Services.AddSingleton(typeof(ILogger), typeof(Afonsoft.Logger.Logger));
            builder.Services.AddSingleton(typeof(IOptions<AfonsoftLoggerOptions>), typeof(AfonsoftLoggerOptions));
            builder.Services.AddOptions<AfonsoftLoggerOptions>();
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider<T>(this ILoggingBuilder builder, AfonsoftLoggerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            builder.Services.AddSingleton<ILoggerProvider>(new AfonsoftLoggerProvider<T>(options));
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Afonsoft.Logger.Logger<>));
            builder.Services.AddSingleton(typeof(ILogger), typeof(Afonsoft.Logger.Logger));
            builder.Services.AddSingleton(typeof(IOptions<AfonsoftLoggerOptions>), options);
            builder.Services.AddOptions<AfonsoftLoggerOptions>();
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
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Afonsoft.Logger.Logger<>));
            builder.Services.AddSingleton(typeof(ILogger), typeof(Afonsoft.Logger.Logger));
            builder.Services.AddSingleton(typeof(IOptions<AfonsoftLoggerOptions>), typeof(AfonsoftLoggerOptions));
            builder.Services.AddOptions<AfonsoftLoggerOptions>();
            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddAfonsoftLoggerProvider(this ILoggingBuilder builder, AfonsoftLoggerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            builder.Services.AddSingleton<ILoggerProvider>(new AfonsoftLoggerProvider(options));
            builder.Services.AddSingleton(typeof(IOptions<AfonsoftLoggerOptions>), options);
            builder.Services.AddSingleton(typeof(ILogger<>), typeof(Afonsoft.Logger.Logger<>));
            builder.Services.AddSingleton(typeof(ILogger), typeof(Afonsoft.Logger.Logger));
            builder.Services.AddOptions<AfonsoftLoggerOptions>();
            return builder;
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
        public static IServiceCollection AddAfonsoftLogging<T>(this IServiceCollection services, AfonsoftLoggerOptions configure)
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
        public static IServiceCollection AddAfonsoftLogging(this IServiceCollection services, AfonsoftLoggerOptions configure)
        {
            services.AddLogging(logging =>
            {
                logging.AddAfonsoftLoggerProvider(configure);
            });

            
            return services;
        }
    }
}
