using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace UniversalHelmod.Classes
{
    public class Logger
    {
        public static string LOGGER_NAME = "UniversalHelmod";

        private static Logger theInstance;
        public static Logger Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new Logger();
                    theInstance.Initialize();
                }
                return theInstance;
            }
        }
        private Logger() { }
        private CustomLogger logger;
        public void Initialize()
        {
            this.logger = new CustomLogger();
            this.logger.Initialize("Helmod");
        }
        public static void SetLogFilePath(string path)
        {
            Instance.logger.SetLogFilePath(path);
        }
        public static void SetLevel(LoggerLevel level)
        {
            Instance.logger.SetLevel(level);
        }
        public static void Info(string message, params string[] args)
        {
            Instance.logger.Info(message, args);
        }
        public static void Debug(string message, params string[] args)
        {
            Instance.logger.Debug(message, args);
        }
        public static void Trace(string message, params string[] args)
        {
            Instance.logger.Trace(message, args);
        }
        public static void Warn(string message, params string[] args)
        {
            Instance.logger.Warn(message, args);
        }
        public static void Critical(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            Instance.logger.Critical(ex, fname, fpath, line);
        }
        public static void Critical(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            Instance.logger.Critical(message, fname, fpath, line);
        }
        public static void Error(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            Instance.logger.Error(ex, fname, fpath, line);
        }
        public static void Error(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            Instance.logger.Error(message, fname, fpath, line);
        }
        public static void ApplicationInfo(System.Reflection.Assembly callingAssembly)
        {
            Instance.logger.ApplicationInfo(callingAssembly);
        }
    }
    public enum LoggerLevel
    {
        Off,
        Critical,
        Error,
        Warn,
        Info,
        Debug,
        Trace,
        All
    }
    public interface ILogger
    {
        void SetLevel(LoggerLevel level);
        void Info(string message, params string[] args);
        void Debug(string message, params string[] args);
        void Trace(string message, params string[] args);
        void Warn(string message, params string[] args);
        void Critical(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0);
        void Critical(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0);
        void Error(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0);
        void Error(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0);
    }
    public class CustomLogger : ILogger, IDisposable
    {
        public CustomLogger() { }
        private string name = "CustomLogger";
        private string path;
        private TextWriter fileWriter;
        private LoggerLevel loggerLevel = LoggerLevel.Info;
        public void Initialize(string name)
        {
            try
            {
                var config = LoggerConfig.ReadXml();
                string path = Path.Combine(Path.GetTempPath(), config.Filename);
                Initialize(path, name);
                this.loggerLevel = config.LogLevel;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }
        public string FilePath => this.path;
        public void Initialize(string path, string name)
        {
            this.path = path;
            this.name = name;
        }
        public void SetLogFilePath(string path)
        {
            this.path = path;
        }

        public void SetLevel(LoggerLevel level)
        {
            this.loggerLevel = level;
        }
        internal void WriteWithCaller(LoggerLevel level, Exception ex, string fname, string fpath, int line)
        {
            if (level != LoggerLevel.Off && level <= loggerLevel)
            {
                var content = $"{System.IO.Path.GetFileName(fpath)}:{line}|{ex.GetType().FullName} : {ex.Message}";
                Write(level, content);
                Write(ex.StackTrace);
                Dispose();
            }
        }
        internal void WriteWithCaller(LoggerLevel level, string message, string fname, string fpath, int line)
        {
            if (level != LoggerLevel.Off && level <= loggerLevel)
            {
                var content = $"{System.IO.Path.GetFileName(fpath)}:{line}|{message}";
                Write(level, content);
                Dispose();
            }
        }
        internal void WriteWithArgs(LoggerLevel level, string message, string[] args)
        {
            if (args == null)
            {
                Write(level, message);
                Dispose();
            }
            else
            {
                Write(level, String.Format(message, args));
                Dispose();
            }
        }
        internal void Write(LoggerLevel level, string message)
        {
            try
            {
                if (level != LoggerLevel.Off && level <= loggerLevel)
                {
                    string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ff");
                    string username = $"{Environment.UserDomainName}\\{Environment.UserName}";
                    string content = $"{timestamp}|{level.ToString().ToUpper(),-5}|{this.name}|{username}|{message}";
                    Write(content);
                }
            }
            catch { }
        }
        internal void Write(string message)
        {
            try
            {
                var consoleWriter = Console.Out;
                Write(consoleWriter, message);
            }
            catch { }
            try
            {
                if (this.path != null)
                {
                    if (fileWriter == null)
                    {
                        fileWriter = GetWriteStream(path, 1000);
                    }
                    Write(fileWriter, message);
                }
            }
            catch { }
        }
        internal void Write(TextWriter writer, string message)
        {
            writer.WriteLine(message);
            writer.Flush();
        }
        internal StreamWriter GetWriteStream(string path, int timeoutMs)
        {
            var time = Stopwatch.StartNew();
            while (time.ElapsedMilliseconds < timeoutMs)
            {
                try
                {
                    bool logAlreadyCreated = false;
                    if (File.Exists(path))
                    {
                        var info = new FileInfo(path);
                        if (info.Length > 1024 * 1024)
                        {
                            var folder = Path.GetDirectoryName(path);
                            var newName = Path.GetFileNameWithoutExtension(path);
                            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            newName = $"{newName}.{timestamp}.log";
                            var newPath = Path.Combine(folder, newName);
                            File.Move(path, newPath);
                        }
                        else
                        {
                            logAlreadyCreated = true;
                        }
                    }
                    var writer = new StreamWriter(path, logAlreadyCreated, Encoding.UTF8);
                    return writer;
                }
                catch (IOException e)
                {
                    // access error
                    if (e.HResult != -2147024864)
                        throw;
                }
            }

            throw new TimeoutException($"Failed to get a write handle to {path} within {timeoutMs}ms.");
        }
        public void Info(string message, string[] args = null)
        {
            WriteWithArgs(LoggerLevel.Info, message, args);
        }
        public void Debug(string message, string[] args = null)
        {
            WriteWithArgs(LoggerLevel.Debug, message, args);
        }
        public void Trace(string message, string[] args = null)
        {
            WriteWithArgs(LoggerLevel.Trace, message, args);
        }
        public void Warn(string message, string[] args = null)
        {
            WriteWithArgs(LoggerLevel.Warn, message, args);
        }
        public void Critical(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            WriteWithCaller(LoggerLevel.Critical, ex, fname, fpath, line);
        }
        public void Critical(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            WriteWithCaller(LoggerLevel.Critical, message, fname, fpath, line);
        }
        public void Error(Exception ex, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            WriteWithCaller(LoggerLevel.Error, ex, fname, fpath, line);
        }
        public void Error(string message, [CallerMemberName] string fname = "", [CallerFilePath] string fpath = "", [CallerLineNumber] int line = 0)
        {
            WriteWithCaller(LoggerLevel.Error, message, fname, fpath, line);
        }

        public void Dispose()
        {
            try
            {
                if (fileWriter != null)
                {
                    fileWriter.Close();
                    fileWriter = null;
                }
            }
            catch { }
        }
    }
    public static class LoggerExtensions
    {
        public static void ApplicationInfo(this ILogger logger, System.Reflection.Assembly callingAssembly)
        {
            var name = callingAssembly.GetName();
            var now = DateTime.Now.ToString("U");
            logger.Info("*********************************");
            logger.Info($"Application     : {name.Name} v{name.Version}: {now}");
            logger.Info($"Executable Path : {callingAssembly.Location}");
            logger.Info($"Thread Culture  : {System.Threading.Thread.CurrentThread.CurrentCulture.DisplayName}");
            logger.Info($"OSVersion       : {Environment.OSVersion}");
            logger.Info($"Machine         : {Environment.MachineName}");
            logger.Info($"User            : {Environment.UserName}");
            logger.Info("*********************************");
        }
    }
    [Serializable]
    [XmlRoot("Root")]
    public class LoggerConfig
    {
        [XmlElement("Filename")]
        public string Filename { get; set; }

        [XmlElement("LogLevel")]
        public LoggerLevel LogLevel { get; set; }

        public const string CONFIG_FOLDER = "Config";
        public static string GetFilename()
        {
            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(root, CONFIG_FOLDER, "Logger.xml");
        }
        public static LoggerConfig ReadXml(string path = null)
        {
            if (path == null) path = GetFilename();
            LoggerConfig configuration = DeserializeItem(path);
            return configuration;
        }
        internal static LoggerConfig DeserializeItem(string fileName)
        {
            try
            {
                FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer deserializer = new XmlSerializer(typeof(LoggerConfig));
                LoggerConfig xmlConfig = (LoggerConfig)deserializer.Deserialize(reader);
                reader.Close();
                return xmlConfig;
            }
            catch
            {
                Console.Out.WriteLine($"Missing Config file: {GetFilename()}");
                var config = new LoggerConfig();
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                config.Filename = $"{name}.log";
                config.LogLevel = LoggerLevel.Info;
                return config;
            }
        }
    }
}
