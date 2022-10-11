using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalHelmod.Classes
{
    public class Logger
    {
        public static string LOGGER_NAME = "UniversalHelmod";
        public static string LOGGER_PATH = "Config/log4net.xml";

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
            this.logger.Initialize(Logger.LOGGER_PATH, Logger.LOGGER_NAME);
        }
        public static void SetLogFilePath(string path)
        {
            Instance.logger.SetLogFilePath(path);
        }
        public static void AddLogFilePath(string path, string pattern)
        {
            Instance.logger.AddLogFilePath(path, pattern);
        }
        public static void SetLevel(string level)
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
        public static void Critical(Exception e)
        {
            Instance.logger.Critical(e);
        }
        public static void Critical(string message, params string[] args)
        {
            Instance.logger.Critical(message, args);
        }
        public static void Error(Exception e)
        {
            Instance.logger.Error(e);
        }
        public static void Error(string message, params string[] args)
        {
            Instance.logger.Error(message, args);
        }

    }
    public class CustomLogger
    {
        public CustomLogger() { }
        private ILog log;
        private string path;
        public void Initialize(string path, string name)
        {
            if (log != null) return;
            log = LogManager.GetLogger(name);
            var file = new System.IO.FileInfo(path);
            log4net.Config.XmlConfigurator.Configure(file);
        }
        public void SetLogFilePath(string path)
        {
            this.path = path;
            var hierarchy = LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            foreach (IAppender appender in hierarchy.Root.Appenders)
            {
                if (appender is FileAppender)
                {
                    FileAppender fileAppender = appender as FileAppender;
                    fileAppender.File = path;
                    fileAppender.ActivateOptions();
                    break;
                }
            }
        }
        public void AddLogFilePath(string path, string pattern)
        {
            this.path = path;
            var logger = log.Logger as log4net.Repository.Hierarchy.Logger;
            IAppender newRollingFile = CreateFileAppender("RollingFile", path, pattern);
            logger.AddAppender(newRollingFile);
        }
        public void RemoveLogFilePath(string path = null)
        {
            if (path == null) path = this.path;
            var logger = log.Logger as log4net.Repository.Hierarchy.Logger;
            foreach (IAppender appender in logger.Appenders)
            {
                if (appender is FileAppender)
                {
                    FileAppender fileAppender = appender as FileAppender;
                    if (fileAppender.File == path)
                    {
                        fileAppender.Close();
                        logger.RemoveAppender(appender);
                        break;
                    }
                }
            }
        }
        public IAppender CreateFileAppender(string name, string fileName, string pattern, bool append = true)
        {
            FileAppender appender = new FileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = append;
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();
            layout.ConversionPattern = pattern;
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();
            return appender;
        }
        public void SetLevel(string level)
        {
            log4net.Core.Level rootLevel = log4net.Core.Level.All;
            switch (level.ToLower())
            {
                case "critical":
                    rootLevel = log4net.Core.Level.Critical;
                    break;
                case "error":
                    rootLevel = log4net.Core.Level.Error;
                    break;
                case "warning":
                    rootLevel = log4net.Core.Level.Warn;
                    break;
                case "info":
                    rootLevel = log4net.Core.Level.Info;
                    break;
                case "debug":
                    rootLevel = log4net.Core.Level.Debug;
                    break;
                case "trace":
                    rootLevel = log4net.Core.Level.Trace;
                    break;
                case "all":
                    rootLevel = log4net.Core.Level.All;
                    break;
                case "off":
                    rootLevel = log4net.Core.Level.Off;
                    break;
            }
            var hierarchy = this.log.Logger.Repository as log4net.Repository.Hierarchy.Hierarchy;
            hierarchy.Root.Level = rootLevel;
        }
        public void Info(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Info(message);
            }
            else
            {
                this.log.InfoFormat(message, args);
            }
        }
        public void Debug(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Debug(message);
            }
            else
            {
                this.log.DebugFormat(message, args);
            }
        }
        public void Trace(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Logger.Log(null, log4net.Core.Level.Trace, message, null);
            }
            else
            {
                this.log.Logger.Log(null, log4net.Core.Level.Trace, String.Format(message, args), null);
            }
        }
        public void Warn(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Warn(message);
            }
            else
            {
                this.log.WarnFormat(message, args);
            }
        }
        public void Critical(Exception e)
        {
            this.log.Logger.Log(null, log4net.Core.Level.Critical, e, null);
        }
        public void Critical(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Logger.Log(null, log4net.Core.Level.Critical, message, null);
            }
            else
            {
                this.log.Logger.Log(null, log4net.Core.Level.Critical, String.Format(message, args), null);
            }
        }
        public void Error(Exception e)
        {
            this.log.Error(e);
        }
        public void Error(string message, params string[] args)
        {
            if (args == null)
            {
                this.log.Error(message);
            }
            else
            {
                this.log.ErrorFormat(message, args);
            }
        }

    }
}
