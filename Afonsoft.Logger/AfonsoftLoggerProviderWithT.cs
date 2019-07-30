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
    public class AfonsoftLoggerProvider<T> : BatchingLoggerProvider
    {
        private static AfonsoftLoggerOptions afonsoftLoggerOptions = new AfonsoftLoggerOptions();
        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(IOptionsMonitor<AfonsoftLoggerOptions> options) : base(options)
        {
            afonsoftLoggerOptions = options.CurrentValue;
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(Func<AfonsoftLoggerOptions> options): base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(options.Invoke()))
        {
            afonsoftLoggerOptions = options.Invoke();
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(AfonsoftLoggerOptions options) : base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(options))
        {
            afonsoftLoggerOptions = options;
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        public AfonsoftLoggerProvider() : base(new OptionsWrapperMonitor<AfonsoftLoggerOptions>(afonsoftLoggerOptions))
        {
  
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
        /// 
        /// </summary>
        /// <returns></returns>
        public ILogger<T> CreateLogger()
        {
            return new Afonsoft.Logger.Logger<T>(this);
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