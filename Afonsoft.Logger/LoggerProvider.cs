using Microsoft.Extensions.Logging;
using System;

namespace Afonsoft.Logger
{
    /// <summary>
    /// LoggerProvider : ILoggerProvider 
    /// </summary>
    public class LoggerProvider<T> : ILoggerProvider, ISupportExternalScope
    {
        private Func<string, LogLevel, bool> _filter;
        private LoggerRepository _repository;
        private string _categoryName;

        /// <summary>
        /// IncludeScopes
        /// </summary>
        public bool IncludeScopes { get; set; } = false;

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            // Not going to use it because IncludeScopes = false
            IncludeScopes = false;
        }

        /// <summary>
        /// LoggerProvider
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="filter"></param>
        public LoggerProvider(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = categoryName ?? typeof(T).ToString();
            _repository = new LoggerRepository();
        }
        /// <summary>
        /// LoggerProvider
        /// </summary>
        /// <param name="filter"></param>
        public LoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = typeof(T).ToString();
            _repository = new LoggerRepository();
        }

        /// <summary>
        /// LoggerProvider
        /// </summary>
        public LoggerProvider()
        {
            _categoryName = typeof(T).ToString();
            _repository = new LoggerRepository();
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            _repository = new LoggerRepository();
            return new Logger<T>( _repository, _filter, categoryName ?? _categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <returns></returns>
        public ILogger CreateLogger()
        {
            _repository = new LoggerRepository();
            return new Logger<T>( _repository, _filter, _categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _repository = new LoggerRepository();
            return new Logger<T>( _repository, filter ?? _filter, categoryName ?? _categoryName);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _repository?.Dispose();
            _repository = null;
        }

    }
}