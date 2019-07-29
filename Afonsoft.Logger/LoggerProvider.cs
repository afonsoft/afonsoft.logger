using Microsoft.Extensions.Logging;
using System;

namespace Afonsoft.Logger
{
    /// <summary>
    /// new Afonsoft.Logger.LoggerProvider<Program>().CreateLogger()
    /// LoggerProvider : ILoggerProvider 
    /// </summary>
    public class AfonsoftLoggerProvider<T> : ILoggerProvider, ISupportExternalScope
    {
        private Func<string, LogLevel, bool> _filter;
        private readonly LoggerRepository _repository;
        private readonly string _categoryName;

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
        public AfonsoftLoggerProvider(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = categoryName ?? typeof(T).ToString();
            _repository = new LoggerRepository();
        }
        /// <summary>
        /// LoggerProvider
        /// </summary>
        /// <param name="filter"></param>
        public AfonsoftLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = typeof(T).ToString();
            _repository = new LoggerRepository();
        }

        /// <summary>
        /// LoggerProvider
        /// </summary>
        public AfonsoftLoggerProvider()
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
            return new Logger<T>( _repository, _filter, categoryName ?? _categoryName);
        }


        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <returns></returns>
        public ILogger CreateLogger()
        {
            return new Logger<T>( _repository, _filter, _categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public ILogger<T1> CreateLogger<T1>()
        {
            return new Logger<T1>(_repository, _filter, typeof(T1).ToString());
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
            return new Logger<T>( _repository, filter ?? _filter, categoryName ?? _categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="categoryName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ILogger<T1> CreateLogger<T1>(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            return new Logger<T1>(_repository, filter ?? _filter, typeof(T1).ToString());
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _repository?.Dispose();
        }

    }
}