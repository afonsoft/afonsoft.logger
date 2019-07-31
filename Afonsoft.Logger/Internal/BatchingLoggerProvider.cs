using Afonsoft.Logger.Rolling;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Afonsoft.Logger.Internal
{

    /// <summary>
    /// BatchingLoggerProvider
    /// </summary>
    public abstract class BatchingLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private const int MaxEventLogEntryLength = 30000;
        private readonly List<LogMessage> _currentBatch = new List<LogMessage>();
        private readonly TimeSpan _interval;
        private readonly int? _queueSize;
        private readonly int? _batchSize;
        private readonly IDisposable _optionsChangeToken;

        private string _path;
        private readonly string _fileName;
        private readonly string _extension;
        private readonly int? _maxFileSize;
        private readonly int? _maxRetainedFiles;
        private readonly PeriodicityOptions _periodicity;

        private BlockingCollection<LogMessage> _messageQueue;
        private Task _outputTask;
        private CancellationTokenSource _cancellationTokenSource;

        private bool _includeScopes;
        private IExternalScopeProvider _scopeProvider;

        internal IExternalScopeProvider ScopeProvider => _includeScopes ? _scopeProvider : null;

        /// <summary>
        /// BatchingLoggerProvider
        /// </summary>
        /// <param name="options"></param>
        protected BatchingLoggerProvider(IOptionsMonitor<AfonsoftLoggerOptions> options)
        {
            // NOTE: Only IsEnabled and IncludeScopes are monitored

            var loggerOptions = options.CurrentValue;
            if (loggerOptions.BatchSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.BatchSize), $"{nameof(loggerOptions.BatchSize)} must be a positive number.");
            }
            if (loggerOptions.FlushPeriod <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(loggerOptions.FlushPeriod), $"{nameof(loggerOptions.FlushPeriod)} must be longer than zero.");
            }

            _interval = loggerOptions.FlushPeriod;
            _batchSize = loggerOptions.BatchSize;
            _queueSize = loggerOptions.BackgroundQueueSize;

            _path = loggerOptions.LogDirectory;
            _fileName = loggerOptions.FileName;
            _extension = loggerOptions.Extension;
            _maxFileSize = loggerOptions.FileSizeLimit;
            _maxRetainedFiles = loggerOptions.RetainedFileCountLimit;
            _periodicity = loggerOptions.Periodicity;
            LogLevel = loggerOptions.LogLevel;

            _optionsChangeToken = options.OnChange(UpdateOptions);
            UpdateOptions(options.CurrentValue);
        }

        /// <summary>
        /// IsEnabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// LogLevel
        /// </summary>
        public LogLevel LogLevel { get; private set; }


        private void UpdateOptions(AfonsoftLoggerOptions options)
        {
            var oldIsEnabled = IsEnabled;
            IsEnabled = options.IsEnabled;
            _includeScopes = options.IncludeScopes;
            LogLevel = options.LogLevel;
            if (oldIsEnabled != IsEnabled)
            {
                if (IsEnabled)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }

        }


        private string ClearPath(string _path)
        {
            if (_path.IndexOf("file:\\", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("file:\\", "");

            if (_path.IndexOf("bin", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\bin", "");

            if (_path.IndexOf("Debug", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\Debug", "");

            if (_path.IndexOf("Release", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\Release", "");

            if (_path.IndexOf("netcoreapp2.2", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\netcoreapp2.2", "");

            if (_path.IndexOf("netstandard2.0", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\netstandard2.0", "");

            if (_path.IndexOf("net47", StringComparison.Ordinal) >= 0)
                _path = _path.Replace("\\net47", "");
            return _path;
        }
        /// <summary>
        /// WriteMessagesAsync
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected async Task WriteMessagesAsync(IEnumerable<LogMessage> messages, CancellationToken token)
        {
            
            foreach (var group in messages.GroupBy(GetGrouping))
            {
               
                foreach (var item in group)
                {

                    try
                    {
                        string StackTraces = "";
                        string ExceptionMessages = "";
                        string _categoryName = item.CategoryName;
                        Type typeObj = typeof(BatchingLoggerProvider);

                        if (item.Type != null && item.Type.Name != "Object")
                            typeObj = item.Type;
                        else if (item.TypeTState != null && item.TypeTState.Name != "Object")
                            typeObj = item.TypeTState;
                        else if (item.MethodBase != null && item.MethodBase.DeclaringType.Name != "Object")
                            typeObj = item.MethodBase.DeclaringType;

                            #region Assembly
                        Assembly assembly;

                        try
                        {
                            if (!string.IsNullOrEmpty(item.CategoryName))
                                assembly = Assembly.Load(item.CategoryName);
                            else
                                assembly = Assembly.GetAssembly(typeObj);
                        }
                        catch
                        {
                            try
                            {
                                assembly = Assembly.GetAssembly(typeObj);
                            }
                            catch
                            {
                                assembly = Assembly.GetCallingAssembly();
                            }
                        }
                         
                        var SystemName = assembly.GetName().Name;
                        var SystemVersion = assembly.GetName().Version.ToString();
                        string methodBaseArgs = "";

                        if (item.MethodBase != null)
                        {
                            foreach (var args in item.MethodBase.GetParameters())
                                methodBaseArgs += args.ToString() + ", ";
                            if(!string.IsNullOrEmpty(methodBaseArgs))
                                methodBaseArgs = methodBaseArgs.Substring(0, methodBaseArgs.Length - 2);
                        }

                        var ClassName = item.MethodBase != null && typeObj != null ? typeObj.Name + "." + item.MethodBase.Name + "(" + methodBaseArgs + ")" : (typeObj != null ? typeObj.Name : SystemName);
                        #endregion

                        #region Exception
                        Exception TmpException = item.Exception;

                        if (TmpException != null)
                        {
                            while (TmpException != null)
                            {
                                string Traces = "";
                                try
                                {
                                    StackFrame[] trace = new StackTrace(TmpException, true).GetFrames();
                                    if (trace != null)
                                    {
                                        foreach (StackFrame stack in trace)
                                        {
                                            if (stack.GetFileLineNumber() > 0 && stack.GetMethod() != null)
                                            {
                                                Traces += $"--> METHOD: {stack.GetMethod().Name} ({stack.GetFileLineNumber()},{stack.GetFileColumnNumber()}) ";
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    //
                                }
                                ExceptionMessages += TmpException.Message + " ";
                                StackTraces += String.IsNullOrEmpty(Traces) ? TmpException.StackTrace : Traces;
                                TmpException = TmpException.InnerException;
                            }
                        }
                        #endregion

                        string pathExe = Path.Combine(ClearPath(Path.Combine(Path.GetDirectoryName(assembly.GetName().CodeBase))), "LOGS");

                        try
                        {
                            _path = ClearPath(_path);
                            Directory.CreateDirectory(_path);
                        }
                        catch
                        {
                            _path = ClearPath(pathExe);
                            Directory.CreateDirectory(_path);
                        }

                        var fullName = GetFullName(group.Key);
                        var fileInfo = new FileInfo(fullName);
                        bool writeFile = true;
                        if (_maxFileSize > 0 && fileInfo.Exists && fileInfo.Length > _maxFileSize)
                        {
                            writeFile = false;
                        }

                        if (writeFile)
                        {
                            using (var streamWriter = File.AppendText(fullName))
                            {
                                #region StreamWriter
                                string messageToSave;
                                if (!string.IsNullOrEmpty(item.Message))
                                {
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + item.DebugLevel + " | " + SystemVersion + " | " + ClassName + " | " + FixString(item.Message);
                                    await streamWriter.WriteLineAsync(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }

                                if (!String.IsNullOrEmpty(ExceptionMessages))
                                {
                                    //|  INFORMATION  |
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + "EXCEPTION    | " + SystemVersion + " | " + ClassName + " | " + FixString(ExceptionMessages);
                                    await streamWriter.WriteLineAsync(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }

                                if (!String.IsNullOrEmpty(StackTraces))
                                {
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + "STACK        | " + SystemVersion + " | " + ClassName + " | " + FixString(StackTraces);
                                    await streamWriter.WriteLineAsync(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }
                                #endregion
                            }
                        }

                        #region EventView only in FW4.7
                        if (string.IsNullOrWhiteSpace(SystemName) && typeObj != null)
                            SystemName = item.MethodBase != null ? typeObj.Name : SystemName;

                        if (string.IsNullOrWhiteSpace(SystemName))
                            SystemName = "Afonsoft.Logger";

                        if (CheckSourceExists(SystemName, SystemName))
                        {
                            if (!string.IsNullOrEmpty(item.Message))
                            {
#if NET47
                                    EventLog.WriteEntry(SystemName, EnsureLogMessageLimit(item.Message), EventLogEntryType.Information);
#endif
                            }

                            if (!string.IsNullOrEmpty(StackTraces))
                            {
#if NET47
                                    EventLog.WriteEntry(SystemName, EnsureLogMessageLimit(StackTraces), EventLogEntryType.Warning);
#endif
                            }

                            if (!string.IsNullOrEmpty(ExceptionMessages))
                            {
#if NET47
                                    EventLog.WriteEntry(SystemName, EnsureLogMessageLimit(ExceptionMessages), EventLogEntryType.Error);
#endif
                            }
                        }
                        #endregion

                    }
                    catch
                    {
                        //ignore
                    }

                }
            }

            RollFiles();
        }

        private bool CheckSourceExists(string source, string eventLogName)
        {
#if NET47 
            if (EventLog.SourceExists(source))
            {
                EventLog evLog = new EventLog { Source = source };
                if (evLog.Log != eventLogName)
                {
                    EventLog.DeleteEventSource(source);
                }
            }

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, eventLogName);
                EventLog.WriteEntry(source, $"Event Log Created '{eventLogName}'/'{source}'", EventLogEntryType.Information);
            }

            return EventLog.SourceExists(source);
#else
            return false;
#endif
        }


        private string FixString(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return "";

            return value.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ").Trim();
        }

        private string EnsureLogMessageLimit(string logMessage)
        {
            if (logMessage.Length > MaxEventLogEntryLength)
            {
                string truncateWarningText = $"... | Log Truncated [ Limit: {MaxEventLogEntryLength} ]";
                // Set the message to the max minus enough room to add the truncate warning.
                logMessage = logMessage.Substring(0, MaxEventLogEntryLength - truncateWarningText.Length);
                logMessage = $"{logMessage}{truncateWarningText}";
            }
            return logMessage;
        }

        private string GetFullName((int Year, int Month, int Day, int Hour, int Minute) group)
        {
            switch (_periodicity)
            {
                case PeriodicityOptions.Minutely:
                    return Path.Combine(_path, $"{_fileName}{group.Year:0000}-{group.Month:00-}{group.Day:00}-{group.Hour:00}-{group.Minute:00}.{_extension}");
                case PeriodicityOptions.Hourly:
                    return Path.Combine(_path, $"{_fileName}{group.Year:0000}-{group.Month:00}-{group.Day:00}-{group.Hour:00}.{_extension}");
                case PeriodicityOptions.Daily:
                    return Path.Combine(_path, $"{_fileName}{group.Year:0000}-{group.Month:00}-{group.Day:00}.{_extension}");
                case PeriodicityOptions.Monthly:
                    return Path.Combine(_path, $"{_fileName}{group.Year:0000}-{group.Month:00}.{_extension}");
            }
            throw new InvalidDataException("Invalid periodicity");
        }

        private (int Year, int Month, int Day, int Hour, int Minute) GetGrouping(LogMessage message)
        {
            return (message.Timestamp.Year, message.Timestamp.Month, message.Timestamp.Day, message.Timestamp.Hour, message.Timestamp.Minute);
        }

        /// <summary>
        /// Deletes old log files, keeping a number of files defined by <see cref="AfonsoftLoggerOptions.RetainedFileCountLimit" />
        /// </summary>
        protected void RollFiles()
        {
            try
            {
                if (_maxRetainedFiles > 0)
                {
                    var files = new DirectoryInfo(_path)
                        .GetFiles(_fileName + "*")
                        .OrderByDescending(f => f.Name)
                        .Skip(_maxRetainedFiles.Value);

                    foreach (var item in files)
                    {
                        item.Delete();
                    }
                }
            }
            catch
            {
                //Ignore
            }
        }

        private async Task ProcessLogQueue()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var limit = _batchSize ?? int.MaxValue;

                while (limit > 0 && _messageQueue.TryTake(out var message))
                {
                    _currentBatch.Add(message);
                    limit--;
                }

                if (_currentBatch.Count > 0)
                {
                    try
                    {
                        await WriteMessagesAsync(_currentBatch, _cancellationTokenSource.Token);
                    }
                    catch(Exception)
                    {
                        RollFiles();
                    }

                    _currentBatch.Clear();
                }

                await IntervalAsync(_interval, _cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// IntervalAsync
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }

        internal void AddMessage(DateTimeOffset timestamp, LogMessage message)
        {
            if (!_messageQueue.IsAddingCompleted)
            {
                try
                {
                    _messageQueue.Add(message, _cancellationTokenSource.Token);
                }
                catch
                {
                    //cancellation token canceled or CompleteAdding called
                }
            }
        }

        private void Start()
        {
            _messageQueue = _queueSize == null ?
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>()) :
                new BlockingCollection<LogMessage>(new ConcurrentQueue<LogMessage>(), _queueSize.Value);

            _cancellationTokenSource = new CancellationTokenSource();
            _outputTask = Task.Run(ProcessLogQueue);
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _messageQueue.CompleteAdding();

            try
            {
                _outputTask.Wait(_interval);
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
            {
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _optionsChangeToken?.Dispose();
            if (IsEnabled)
            {
                Stop();
            }
        }

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public abstract ILogger CreateLogger(string categoryName);

        /// <summary>
        /// CreateLogger
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract ILogger<T> CreateLogger<T>();


        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
}