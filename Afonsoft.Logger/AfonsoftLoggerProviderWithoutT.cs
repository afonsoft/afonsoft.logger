using Afonsoft.Logger.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Afonsoft.Logger
{
    /// <summary>
    ///AfonsoftLoggerProvider
    /// LoggerProvider : ILoggerProvider 
    /// </summary>
    public class AfonsoftLoggerProvider : BatchingLoggerProvider
    {

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(IOptionsMonitor<AfonsoftLoggerOptions> options) : base(options)
        {
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(Action<AfonsoftLoggerOptions> options) : base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(Build(options)))
        {
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(AfonsoftLoggerOptions options) : base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(options))
        {
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        public AfonsoftLoggerProvider() : base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(new AfonsoftLoggerOptions()))
        {

        }

        private static AfonsoftLoggerOptions Build(Action<AfonsoftLoggerOptions> configure)
        {
            var expr = new AfonsoftLoggerOptions();
            configure(expr);
            return expr;
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public override ILogger CreateLogger(string categoryName)
        {
            return new Afonsoft.Logger.Logger(this, categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public override ILogger<T1> CreateLogger<T1>()
        {
            return new Afonsoft.Logger.Logger<T1>(this);
        }
    }
}