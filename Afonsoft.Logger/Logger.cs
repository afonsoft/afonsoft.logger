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
    /// Classe para efetuar o Log
    /// HH:MM:SS | EXCEPTION | VERSION | CLASS NAME AND METHOD | ERROR MENSSAGE
    /// </summary>
    public class Logger<T> : ILogger<T>
    {
        private IExternalScopeProvider ScopeProvider { get; set; }
        private Func<string, LogLevel, bool> _filter;
        private string _categoryName;
        private LoggerRepository _repository;

        private Logger()
        {
            _categoryName = typeof(T).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="filter"></param>
        /// <param name="categoryName"></param>
        public Logger(LoggerRepository repository, Func<string, LogLevel, bool> filter, string categoryName)
        {
            _repository = repository;
            _filter = filter;
            _categoryName = categoryName;
        }

        private void LogFile<TState>(string categoryName, LogLevel logLevel, string message, Exception exception, params object[] debugData)
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

            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(categoryName, stackTrace.GetFrame(stackTrace.FrameCount - 1).GetMethod(), type, message, exception, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(categoryName, null, type, message, exception, debugData);
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

            GetScope(logBuilder);

            LogFile<TState>(_categoryName, logLevel, logBuilder.ToString(), exception, null);
        }

        private void GetScope(StringBuilder stringBuilder)
        {
            var scopeProvider = ScopeProvider;
            if (scopeProvider != null)
            {
                var initialLength = stringBuilder.Length;

                scopeProvider.ForEachScope((scope, state) =>
                {
                    var (builder, length) = state;
                    var first = length == builder.Length;
                    builder.Append(first ? "=> " : " => ").Append(scope);
                }, (stringBuilder, initialLength));

                stringBuilder.AppendLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || logLevel != LogLevel.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
       

        private string MyFormatter<TState>(TState state, Exception exception)
        {
            // create some string
            return exception?.Message;
        }
    }
}