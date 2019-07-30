using Afonsoft.Logger.Rolling;
using System;
using System.IO;
using System.Reflection;

namespace Afonsoft.Logger
{
    /// <summary>
    /// Options for file logging.
    /// </summary>
    public class AfonsoftLoggerOptions 
    {
        private int? _fileSizeLimit = 40 * 1024 * 1024;
        private int? _retainedFileCountLimit = 2;
        private string _fileName = "";
        private string _extension = "log";
        private int? _batchSize = null;
        private int? _backgroundQueueSize = 1000;
        private TimeSpan _flushPeriod = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the period after which logs will be flushed to the store.
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get { return _flushPeriod; }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FlushPeriod)} must be positive.");
                }
                _flushPeriod = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the background log message queue or null for no limit.
        /// After maximum queue size is reached log event sink would start blocking.
        /// Defaults to <c>1000</c>.
        /// </summary>
        public int? BackgroundQueueSize
        {
            get { return _backgroundQueueSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BackgroundQueueSize)} must be non-negative.");
                }
                _backgroundQueueSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a maximum number of events to include in a single batch or null for no limit.
        /// </summary>
        /// Defaults to <c>null</c>.
        public int? BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BatchSize)} must be positive.");
                }
                _batchSize = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating if logger accepts and queues writes.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether scopes should be included in the message.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool IncludeScopes { get; set; } = false;

        /// <summary>
        /// Gets or sets a strictly positive value representing the maximum log size in bytes or null for no limit.
        /// Once the log is full, no more messages will be appended.
        /// Defaults to <c>40MB</c>.
        /// </summary>
        public int? FileSizeLimit
        {
            get { return _fileSizeLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(FileSizeLimit)} must be positive.");
                }
                _fileSizeLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets a strictly positive value representing the maximum retained file count or null for no limit.
        /// Defaults to <c>2</c>.
        /// </summary>
        public int? RetainedFileCountLimit
        {
            get { return _retainedFileCountLimit; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(RetainedFileCountLimit)} must be positive.");
                }
                _retainedFileCountLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets the filename prefix to use for log files.
        /// Defaults to <c>logs-</c>.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                _fileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the filename extension to use for log files.
        /// Defaults to <c>txt</c>.
        /// Will strip any prefixed .
        /// </summary>
        public string Extension
        {
            get { return _extension; }
            set { _extension = value?.TrimStart('.'); }
        }

        /// <summary>
        /// Gets or sets the periodicity for rolling over log files.
        /// </summary>
        public PeriodicityOptions Periodicity { get; set; } = PeriodicityOptions.Daily;

        /// <summary>
        /// The directory in which log files will be written, relative to the app process.
        /// Default to <c>Logs</c>
        /// </summary>
        /// <returns></returns>
        public string LogDirectory { get; set; } = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().GetName().CodeBase), "LOGS");
    }
}
