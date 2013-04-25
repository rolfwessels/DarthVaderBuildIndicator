using System;

namespace BuildIndicatron.App.Core.Log
{
    public interface ILog
    {
        void Error(string message);
        void Error(string message, Exception exception);
        void ErrorException(string message, Exception exception);
        void Info(string message);
        void Info(string messsage, params object[] values);

        void Debug(string message);

        void Debug(string messsage, params object[] values);

        void Warn(string message);
        void Warn(string message, params object[] values);
    }

    public abstract class Logger : ILog
    {
        public const string ErrorLevel = "ERROR";
        public const string InfoLevel = "INFO";
        public const string DebugLevel = "DEBUG";
        public const string WarnLevel = "WARN";

        protected string ClassName { get; set; }

        public abstract void LogMethod(string level, string logger, string message, Exception exception);

        public void Error(string message)
        {
            LogMethod(ErrorLevel, ClassName, message, null);
        }

        public void Error(string message, Exception exception)
        {
            LogMethod(ErrorLevel, ClassName, message, exception);
        }

        public void ErrorException(string message, Exception exception)
        {
            LogMethod(ErrorLevel, ClassName, message, exception);
        }

        public void Info(string message)
        {
            LogMethod(InfoLevel, ClassName, message, null);
        }

        public void Info(string messsage, params object[] values)
        {
            LogMethod(InfoLevel, ClassName, string.Format(messsage,values), null);
        }

        public void Debug(string message)
        {
            LogMethod(DebugLevel, ClassName, message, null);
        }

        public void Debug(string messsage, params object[] values)
        {
            LogMethod(DebugLevel, ClassName, string.Format(messsage, values), null);
        }

        public void Warn(string message)
        {
            LogMethod(WarnLevel, ClassName, message, null);
        }

        public void Warn(string message, params object[] values)
        {
            LogMethod(WarnLevel, ClassName, string.Format(message, values), null);
        }

   
    }
}