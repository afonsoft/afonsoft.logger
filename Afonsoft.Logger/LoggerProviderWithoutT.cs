﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.Logger
{
    /// <summary>
    /// new Afonsoft.Logger.LoggerProvider<Program>().CreateLogger()
    /// LoggerProvider : ILoggerProvider 
    /// </summary>
    public class LoggerProvider : ILoggerProvider, ISupportExternalScope
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
            _categoryName = categoryName;
            _repository = new LoggerRepository();
        }

        /// <summary>
        /// LoggerProvider
        /// </summary>
        private LoggerProvider()
        {
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
            return new Logger(_repository, _filter, categoryName ?? _categoryName);
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <returns></returns>
        public ILogger CreateLogger()
        {
            _repository = new LoggerRepository();
            return new Logger(_repository, _filter, _categoryName);
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
            return new Logger(_repository, filter ?? _filter, categoryName ?? _categoryName);
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