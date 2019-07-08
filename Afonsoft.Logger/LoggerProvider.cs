using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft
{
    /// <summary>
    /// LoggerProvider : ILoggerProvider 
    /// </summary>
    public class LoggerProvider : ILoggerProvider
    {
        private Func<string, LogLevel, bool> _filter;
        private LoggerRepository _repository;
        private string _categoryName;

        /// <summary>
        /// LoggerProvider
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="filter"></param>
        public LoggerProvider(string categoryName, Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
            _categoryName = categoryName;
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
            return new Logger<LoggerRepository>(_repository, (category, logLevel) => logLevel >= LogLevel.Debug, categoryName);
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
            return new Logger<LoggerRepository>(_repository, filter, categoryName);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}