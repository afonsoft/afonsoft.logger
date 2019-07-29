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
        private static FileLoggerOptions fileLoggerOptions = new FileLoggerOptions();
        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(IOptionsMonitor<FileLoggerOptions> options) : base(options)
        {
            fileLoggerOptions = options.CurrentValue;
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(Func<FileLoggerOptions> options): base(new OptionsWrapperMonitor<FileLoggerOptions>(options.Invoke()))
        {
            fileLoggerOptions = options.Invoke();
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        public AfonsoftLoggerProvider(FileLoggerOptions options) : base(new OptionsWrapperMonitor<FileLoggerOptions>(options))
        {
            fileLoggerOptions = options;
        }

        /// <summary>
        /// AfonsoftLoggerProvider
        /// </summary>
        public AfonsoftLoggerProvider() : base(new OptionsWrapperMonitor<FileLoggerOptions>(fileLoggerOptions))
        {
  
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public override ILogger CreateLogger(string categoryName)
        {
            return new Afonsoft.Logger.Internal.Logger(this, categoryName);
        }

 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILogger<T> CreateLogger()
        {
            return new Afonsoft.Logger.Internal.Logger<T>(this);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public override ILogger<T1> CreateLogger<T1>()
        {
            return new Afonsoft.Logger.Internal.Logger<T1>(this);
        }
    }
}