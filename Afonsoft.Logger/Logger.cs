using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Afonsoft
{
    /// <summary>
    /// Classe para efetuar o Log
    /// HH:MM:SS | EXCEPTION | VERSION | CLASS NAME AND METHOD | ERROR MENSSAGE
    /// </summary>
    public class Logger
    {

        private static readonly object lockObject = new object();
        private const int MaxEventLogEntryLength = 30000;

        private Logger()
        {
        }

        /// <summary>
        /// Criar um arquivo de texto
        /// </summary>
        /// <param name="name">Nome do Arquivo</param>
        /// <param name="content">Conteudo do Arquivo</param>
        /// <returns></returns>
        public static Task<Boolean> WriteFileAsync(string name, string content)
        {
            return WriteFileAsync(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), name, content);
        }

        /// <summary>
        /// Criar um arquivo de texto
        /// </summary>
        /// <param name="path">Caminho do Arquivo</param>
        /// <param name="name">Nome do Arquivo</param>
        /// <param name="content">Conteudo do Arquivo</param>
        /// <returns></returns>
        public static Task<Boolean> WriteFileAsync(string path, string name, string content)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(path))
                        path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

                    if (!string.IsNullOrEmpty(path))
                    {
                        if (path.IndexOf("file:\\", StringComparison.Ordinal) >= 0)
                            path = path.Replace("file:\\", "");

                        if (path.IndexOf("bin", StringComparison.Ordinal) >= 0)
                            path = path.Replace("\\bin", "");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        if (string.IsNullOrEmpty(name))
                            name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";

                        string xmlFilePath = Path.Combine(path, name);
                        using (FileStream file = new FileStream(xmlFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            using (StreamWriter sw = new StreamWriter(file, Encoding.UTF8))
                            {
                                sw.Write(content);
                                sw.Flush();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <param name="message">Error Message</param>
        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, null);
            }
            catch
            {
                LogAsync<Logger>(null, "DEBUG    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        [Conditional("DEBUG")]
        public static void Debug<T>(string message) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, null);
            }
            catch
            {
                LogAsync<T>(null, "DEBUG    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        [Conditional("DEBUG")]
        public static void Debug(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, debugData);
            }
            catch
            {
                LogAsync<Logger>(null, "DEBUG    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        [Conditional("DEBUG")]
        public static void Debug<T>(string message, params object[] debugData) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG    ", message, null, debugData);
            }
            catch
            {
                LogAsync<T>(null, "DEBUG    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        public static void Error(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, null);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        public static void Error<T>(string message) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, null);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="exception">Exception</param>
        public static void Error(Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, null);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="exception">Exception</param>
        public static void Error<T>(Exception exception) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, null);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public static void Error(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, debugData);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public static void Error<T>(string message, params object[] debugData) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, null, debugData);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        public static void Error(string message, Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, null);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        public static void Error<T>(string message, Exception exception) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, null);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public static void Error(string message, Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, debugData);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public static void Error<T>(string message, Exception exception, params object[] debugData) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", message, exception, debugData);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public static void Error(Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, debugData);
            }
            catch
            {
                LogAsync<Logger>(null, "ERROR    ", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="exception">Exception Error</param>
        /// <param name="debugData">Object Error</param>
        public static void Error<T>(Exception exception, params object[] debugData) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR    ", null, exception, debugData);
            }
            catch
            {
                LogAsync<T>(null, "ERROR    ", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        public static void Info(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, null);
            }
            catch
            {
                LogAsync<Logger>(null, "INFO     ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        public static void Info<T>(string message) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, null);
            }
            catch
            {
                LogAsync<T>(null, "INFO     ", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>

        public static void Info(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<Logger>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, debugData);
            }
            catch
            {
                LogAsync<Logger>(null, "INFO     ", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="message">Error Message</param>
        /// <param name="debugData">Object Error</param>
        public static void Info<T>(string message, params object[] debugData) where T : class
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                LogAsync<T>(stackTrace.GetFrame(1).GetMethod(), "INFO     ", message, null, debugData);
            }
            catch
            {
                LogAsync<T>(null, "INFO     ", message, null, debugData);
            }
        }

        private static void LogAsync<T>(MethodBase methodBase, string type, string message, Exception exception, params object[] debugData) where T : class
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string StackTraces = "";
                    string ExceptionMessages = "";


                    Type typeObj = methodBase != null ? methodBase.DeclaringType : typeof(T);

                    Assembly assembly;
                    try
                    {
                        assembly = typeObj!= null ? Assembly.GetAssembly(typeObj) : Assembly.GetExecutingAssembly();
                    }
                    catch
                    {
                        assembly = Assembly.GetExecutingAssembly();
                    }

 
                    var SystemName = assembly.GetName().Name;
                    var SystemVersion = assembly.GetName().Version.ToString();

                    var ClassName = methodBase != null && typeObj != null ? typeObj.Name + "." + methodBase.Name + "()" : (typeObj != null ? typeObj.Name : SystemName);

                    string pathExe = Path.GetDirectoryName(assembly.GetName().CodeBase);

                    Exception TmpException = exception;

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

                    if (!string.IsNullOrWhiteSpace(pathExe))
                    {
                        if (pathExe.IndexOf("file:\\", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("file:\\", "");

                        if (pathExe.IndexOf("bin", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\bin", "");

                        string path = Path.Combine(pathExe, "LOGS");

                        if (!Directory.Exists(path))
                        {
                            try
                            {
                                Directory.CreateDirectory(path);
                            }
                            catch
                            {
                                path = pathExe;
                            }
                        }

                        string FileName = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd") + ".log");

                        lock (lockObject)
                        {
                            using (StreamWriter sw = new StreamWriter(FileName, true, Encoding.UTF8))
                            {
                                string messageToSave;
                                if (!string.IsNullOrEmpty(message))
                                {
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + type + " | " + SystemVersion + " | " + ClassName + " | " + FixString(message);
                                    sw.WriteLine(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }

                                if (!String.IsNullOrEmpty(ExceptionMessages))
                                {
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + "EXCEPTION | " + SystemVersion + " | " + ClassName + " | " + FixString(ExceptionMessages);
                                    sw.WriteLine(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }

                                if (!String.IsNullOrEmpty(StackTraces))
                                {
                                    messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + "STACK     | " + SystemVersion + " | " + ClassName + " | " + FixString(StackTraces);
                                    sw.WriteLine(messageToSave);
                                    if (Environment.UserInteractive)
                                    {
                                        Console.WriteLine(messageToSave);
                                        Trace.WriteLine(messageToSave);
                                    }
                                }

                                if (debugData != null)
                                {
                                    foreach (var data in debugData)
                                    {
                                        messageToSave = DateTime.Now.ToString("HH:mm:ss") + " | " + "OBJECT    | " + SystemVersion + " | " + ClassName + " | " + data.GetType().Name + " --> " + JsonConvert.SerializeObject(data);
                                        sw.WriteLine(messageToSave);

                                        if (Environment.UserInteractive)
                                        {
                                            Console.WriteLine(messageToSave);
                                            Trace.WriteLine(messageToSave);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(SystemName) && typeObj != null)
                        SystemName = methodBase != null ? typeObj.Name : SystemName;

                    if (string.IsNullOrWhiteSpace(SystemName))
                        SystemName = "Afonsoft.Logger";

                    if (CheckSourceExists(SystemName, SystemName))
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
#if NET47
                            EventLog.WriteEntry(SystemName, EnsureLogMessageLimit(message), EventLogEntryType.Information);
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

                        if (debugData != null)
                        {
#if NET47
                            foreach (var data in debugData)
                                EventLog.WriteEntry(SystemName, data.GetType().Name + " --> " + JsonConvert.SerializeObject(data), EventLogEntryType.Information);
#endif
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            });
        }

        private static bool CheckSourceExists(string source, string eventLogName)
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

        private static string FixString(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return "";

            return value.Replace(Environment.NewLine, " - ");
        }

        private static string EnsureLogMessageLimit(string logMessage)
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
    }
}