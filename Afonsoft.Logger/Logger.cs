using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Diagnostics;
using System.Text;

namespace Afonsoft
{
    /// <summary>
    /// Classe para efetuar o Log
    /// HH:MM:SS | EXCEPTION | VERSION | CLASS NAME AND METHOD | ERROR MENSSAGE
    /// </summary>
    public class Logger<Trepository> : ILogger where Trepository : LoggerRepository
    {
        private IExternalScopeProvider ScopeProvider { get; set; }
        private Func<string, LogLevel, bool> _filter;
        private string _categoryName;
        private Trepository _repository;

        private Logger()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="filter"></param>
        /// <param name="categoryName"></param>
        public Logger(Trepository repository, Func<string, LogLevel, bool> filter, string categoryName)
        {
            _repository = repository;
            _filter = filter;
            _categoryName = categoryName;
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <param name="message">Error Message</param>
        [Conditional("DEBUG")]
        public  void Debug(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "DEBUG    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        [Conditional("DEBUG")]
        public  void Debug<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<T>(null, "DEBUG    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        [Conditional("DEBUG")]
        public  void Debug(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "DEBUG    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        [Conditional("DEBUG")]
        public  void Debug<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(null, "DEBUG    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        public  void Error(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        public  void Error<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="exception">Exception</param>
        public  void Error(Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, null);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="exception">Exception</param>
        public  void Error<T>(Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, null);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public  void Error(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public  void Error<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        public  void Error(string message, Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, null);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        public  void Error<T>(string message, Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, null);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public  void Error(string message, Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, debugData);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public  void Error<T>(string message, Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public  void Error(Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, debugData);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "ERROR    ", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public  void Error<T>(Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(null, "ERROR    ", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        public  void Info(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "INFO     ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        public  void Info<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, null);
            }
            catch
            {
                _repository.LogAsync<T>(null, "INFO     ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>

        public  void Info(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<Trepository>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<Trepository>(null, "INFO     ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public  void Info<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                _repository.LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, debugData);
            }
            catch
            {
                _repository.LogAsync<T>(null, "INFO     ", message, null, debugData);
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
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.Append(message);
                logBuilder.Append(Environment.NewLine);
            }

            GetScope(logBuilder);

            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Warning:
                    Debug<TState>(logBuilder.ToString(), eventId);
                    break;
                case LogLevel.Critical:
                case LogLevel.Error:
                    Error<TState>(logBuilder.ToString(), exception, new object[] { eventId });
                    break;
                case LogLevel.Information:
                case LogLevel.Trace:
                    Info<TState>(logBuilder.ToString(), eventId);
                    break;
                default:
                    break;
            }
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
            return _filter == null || _filter(_categoryName, logLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

    }
}