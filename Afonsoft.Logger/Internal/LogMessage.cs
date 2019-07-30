using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Afonsoft.Logger.Internal
{
    /// <summary>
    /// LogMessage
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
       
        /// <summary>
        /// CategoryName
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// MethodBase
        /// </summary>
        public MethodBase MethodBase { get; set; }

        /// <summary>
        /// MethodBase
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// TypeTState
        /// </summary>
        public Type TypeTState { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string DebugLevel { get; set; }
        
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}
