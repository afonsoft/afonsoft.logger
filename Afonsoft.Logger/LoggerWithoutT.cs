using Afonsoft.Logger.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Afonsoft.Logger
{
    /// <summary>
    /// new Afonsoft.Logger.LoggerProvider<Program>().CreateLogger()
    /// Classe para efetuar o Log
    /// HH:MM:SS | EXCEPTION | VERSION | CLASS NAME AND METHOD | ERROR MENSSAGE
    /// </summary>
    public class Logger : ILogger
    {
        private string _categoryName;
        private BatchingLoggerProvider _provider;

        /// <summary>
        /// Logger
        /// </summary>
        public Logger()
        {
            _categoryName = typeof(Logger).ToString();
            _provider = new AfonsoftLoggerProvider();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="categoryName"></param>
        public Logger(ILoggerProvider provider, string categoryName)
        {
            _provider = provider as BatchingLoggerProvider;
            _categoryName = categoryName;
        }


        private void LogFile<TState>(string categoryName, LogLevel logLevel, string message, Exception exception)
        {
            string type;
            switch (logLevel)
            {
                case LogLevel.Information:
                    type = " INFORMATION ";
                    break;
                case LogLevel.Error:
                    type = " ERROR       ";
                    break;
                case LogLevel.Critical:
                    type = " CRITICAL    ";
                    break;
                case LogLevel.Debug:
                    type = " DEBUG       ";
                    break;
                case LogLevel.Trace:
                    type = " TRACE       ";
                    break;
                case LogLevel.Warning:
                    type = " WARNING     ";
                    break;
                default:
                    type = " INFORMATION ";
                    break;
            }

            DateTime timestamp = DateTime.Now;
            try
            {
                StackTrace stackTrace = new StackTrace();
                _provider.AddMessage(timestamp, new LogMessage() { Timestamp = timestamp, DebugLevel = type, CategoryName = categoryName, Exception = exception, Message = message, MethodBase = stackTrace.GetFrame(stackTrace.FrameCount - 1).GetMethod(), Type = typeof(TState), TypeTState = typeof(Logger) });
            }
            catch
            {
                _provider.AddMessage(timestamp, new LogMessage() { Timestamp = timestamp, DebugLevel = type, CategoryName = categoryName, Exception = exception, Message = message, MethodBase = null, Type = typeof(TState), TypeTState = typeof(Logger) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logBuilder = new StringBuilder();

            if (!IsEnabled(logLevel))
            {
                return;
            }

            string message;

            if (formatter == null)
            {
                message = MyFormatter(state, exception);
            }
            else
            {
                message = formatter(state, exception);
            }

            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.Append(message);
            }

            LogFile<TState>(_categoryName, logLevel, logBuilder.ToString(), exception);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {

            if (_provider.IsEnabled && logLevel != LogLevel.None)
            {
                return logLevel >= _provider.LogLevel;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state) => _provider.ScopeProvider?.Push(state);

        private string MyFormatter<TState>(TState state, Exception exception)
        {
            // create some string
            return exception?.Message;
        }
    }
}