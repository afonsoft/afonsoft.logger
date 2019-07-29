using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Afonsoft.Logger
{
    /// <summary>
    /// new Afonsoft.Logger.LoggerProvider<Program>().CreateLogger()
    /// </summary>
    public class LoggerRepository : IDisposable
    {
        private readonly object lockObject = new object();
        private const int MaxEventLogEntryLength = 30000;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="categoryName"></param>
        /// <param name="methodBase"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="debugData"></param>
        public void LogAsync<TState>( string categoryName, MethodBase methodBase, string type, string message, Exception exception, params object[] debugData) 
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string StackTraces = "";
                    string ExceptionMessages = "";
                    string _categoryName = categoryName;

                    Type typeObj = methodBase != null ? methodBase.DeclaringType : typeof(TState);

                    Assembly assembly;
                    try
                    {
                        assembly = typeObj != null ? Assembly.GetAssembly(typeObj) : Assembly.GetCallingAssembly();
                    }
                    catch
                    {
                        try
                        {
                            assembly = Assembly.Load(categoryName);
                        }
                        catch
                        {
                            assembly = Assembly.GetCallingAssembly();
                        }
                    }


                    var SystemName = assembly.GetName().Name;
                    var SystemVersion = assembly.GetName().Version.ToString();
                    string methodBaseArgs="";

                    if(methodBase != null) {
                        foreach (var args in methodBase.GetParameters())
                            methodBaseArgs += args.ToString() + ", ";
                        methodBaseArgs = methodBaseArgs.Substring(0, methodBaseArgs.Length - 2);
                    }

                    var ClassName = methodBase != null && typeObj != null ? typeObj.Name + "." + methodBase.Name + "(" + methodBaseArgs + ")" : (typeObj != null ? typeObj.Name : SystemName);

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

                        if (pathExe.IndexOf("Debug", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\Debug", "");

                        if (pathExe.IndexOf("Release", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\Release", "");

                        if (pathExe.IndexOf("netcoreapp2.2", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\netcoreapp2.2", "");

                        if (pathExe.IndexOf("netstandard2.0", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\netstandard2.0", "");

                        if (pathExe.IndexOf("net47", StringComparison.Ordinal) >= 0)
                            pathExe = pathExe.Replace("\\net47", "");

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

            return value.Replace(Environment.NewLine, " ").Replace("\n"," ").Replace("\r", " ").Trim();
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

        private Task<Boolean> WriteFileAsync(string name, string content)
        {
            return WriteFileAsync(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), name, content);
        }

        private Task<Boolean> WriteFileAsync(string path, string name, string content)
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
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}