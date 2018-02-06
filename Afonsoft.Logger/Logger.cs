using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Afonsoft
{
    public class Logger
    {
        private static readonly object lockObject = new object();
        private Logger()
        {
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "DEBUG", message, null, null);
            }
            catch
            {
                Log<Logger>(null, "DEBUG", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG", message, null, null);
            }
            catch
            {
                Log<T>(null, "DEBUG", message, null, null);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "DEBUG", message, null, debugData);
            }
            catch
            {
                Log<Logger>(null, "DEBUG", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log only in Debug Mode
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "DEBUG", message, null, debugData);
            }
            catch
            {
                Log<T>(null, "DEBUG", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, null, null);
            }
            catch
            {
                Log<Logger>(null, "ERROR", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, null, null);
            }
            catch
            {
                Log<T>(null, "ERROR", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", null, exception, null);
            }
            catch
            {
                Log<Logger>(null, "ERROR", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Error<T>(Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", null, exception, null);
            }
            catch
            {
                Log<T>(null, "ERROR", null, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, null, debugData);
            }
            catch
            {
                Log<Logger>(null, "ERROR", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Error<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, null, debugData);
            }
            catch
            {
                Log<T>(null, "ERROR", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(string message, Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, exception, null);
            }
            catch
            {
                Log<Logger>(null, "ERROR", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Error<T>(string message, Exception exception)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, exception, null);
            }
            catch
            {
                Log<T>(null, "ERROR", message, exception, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(string message, Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, exception, debugData);
            }
            catch
            {
                Log<Logger>(null, "ERROR", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Error<T>(string message, Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", message, exception, debugData);
            }
            catch
            {
                Log<T>(null, "ERROR", message, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Error(Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "ERROR", null, exception, debugData);
            }
            catch
            {
                Log<Logger>(null, "ERROR", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Error<T>(Exception exception, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "ERROR", null, exception, debugData);
            }
            catch
            {
                Log<T>(null, "ERROR", null, exception, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Info(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "INFO", message, null, null);
            }
            catch
            {
                Log<Logger>(null, "INFO", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Info<T>(string message)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "INFO", message, null, null);
            }
            catch
            {
                Log<T>(null, "INFO", message, null, null);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        public static void Info(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<Logger>(stackTrace.GetFrame(1).GetMethod(), "INFO", message, null, debugData);
            }
            catch
            {
                Log<Logger>(null, "INFO", message, null, debugData);
            }
        }

        /// <summary>
        /// Create log
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        public static void Info<T>(string message, params object[] debugData)
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                Log<T>(stackTrace.GetFrame(1).GetMethod(), "INFO", message, null, debugData);
            }
            catch
            {
                Log<T>(null, "INFO", message, null, debugData);
            }
        }

        private static void Log<T>(MethodBase methodBase, string type, string message, Exception exception, params object[] debugData)
        {
            try
            {
                string StackTraces = "";
                string ExceptionMessages = "";
                string ClassName = "";
                string SystemName = "";
                string SystemVersion = "";

                Assembly assembly = Assembly.GetExecutingAssembly();
                Type typeObj = methodBase != null ? methodBase.DeclaringType : typeof(T);

                try
                {
                    assembly = Assembly.GetAssembly(typeObj);
                }
                catch
                {
                    assembly = Assembly.GetExecutingAssembly();
                }

                SystemName = assembly.GetName().Name;
                SystemVersion = assembly.GetName().Version.ToString();

                ClassName = methodBase != null ? typeObj.Name + "." + methodBase.Name + "()" : typeObj.Name;

                string pathExe = Path.GetDirectoryName(assembly.GetName().CodeBase);

                Exception TmpException = exception;

                if (TmpException != null)
                {
                    while (TmpException != null)
                    {

                        string Traces = "";
                        try
                        {
                            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(TmpException, true);
                            if (trace != null)
                            {
                                foreach (StackFrame stack in trace.GetFrames())
                                {
                                    if (stack.GetFileLineNumber() > 0 && stack.GetMethod() != null)
                                    {
                                        Traces += String.Format("--> METHOD: {0} ({1},{2}) ", stack.GetMethod().Name, stack.GetFileLineNumber(), stack.GetFileColumnNumber());
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

                if (pathExe.IndexOf("file:\\") >= 0)
                    pathExe = pathExe.Replace("file:\\", "");

                if (pathExe.IndexOf("bin") >= 0)
                    pathExe = pathExe.Replace("\\bin", "");

                string path = Path.Combine(pathExe, "logs");

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

                        if (!string.IsNullOrEmpty(message))
                        {
                            sw.WriteLine(type + " | " + DateTime.Now.ToString("HH:mm:ss") + " | " + ClassName + " | " + SystemVersion + " | " + FixString(message));
                        }
                        if (!String.IsNullOrEmpty(ExceptionMessages))
                        {
                            sw.WriteLine("EXCEPTION | " + DateTime.Now.ToString("HH:mm:ss") + " | " + ClassName + " | " + SystemVersion + " | " + FixString(ExceptionMessages));
                        }

                        if (!String.IsNullOrEmpty(StackTraces))
                        {
                            sw.WriteLine("STACK | " + DateTime.Now.ToString("HH:mm:ss") + " | " + ClassName + " | " + SystemVersion + " | " + FixString(StackTraces));
                        }

                        if (debugData != null)
                        {
                            foreach (var data in debugData)
                                sw.WriteLine("OBJECT | " + DateTime.Now.ToString("HH:mm:ss") + " | " + ClassName + " | " + SystemVersion + " | " + data.GetType().Name + " --> " + JsonConvert.SerializeObject(data));
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private static string FixString(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return "";

            return value.Replace(Environment.NewLine, " - ");
        }
    }
}