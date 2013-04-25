using System;

namespace BuildIndicatron.App.Core.Log
{
    public class LoggerDefault : Logger
    {
        private const int MaxStringLength = 710;
        private readonly string _minimumLevel;

        public LoggerDefault() : this(DebugLevel)
        {
        }

        public LoggerDefault(string minimumLevel)
        {
            if (minimumLevel == null) throw new ArgumentNullException("minimumLevel");
            _minimumLevel = minimumLevel;
        }

        public override void LogMethod(string level, string logger, string message, Exception exception)
        {
            if (MeetsMinimumLevelRequirement(level))
            {
                System.Diagnostics.Debug.WriteLine(GetMessage(level, message));
                if (exception != null)
                {
                    LogException(level, exception);
                }
            }
        }

        private static void LogException(string level, Exception exception)
        {
            foreach (var line in exception.StackTrace.Split('\n'))
            {
                System.Diagnostics.Debug.WriteLine(GetMessage(level, line.Trim()));    
            }
            if (exception.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine(GetMessage(level, exception.InnerException.Message));
                LogException(level, exception.InnerException);
            }

        }

        protected static string GetMessage(string level, string message)
        {
            string format = string.Format("[{1}] {0}", message, level);
            if (format.Length > MaxStringLength)
            {
                format = format.Substring(0, MaxStringLength);
            }
            return format;
        }

        protected bool MeetsMinimumLevelRequirement(string level)
        {
            if (_minimumLevel == DebugLevel) return true;
            if (_minimumLevel == InfoLevel && level != DebugLevel) return true;
            if (_minimumLevel == WarnLevel && level != DebugLevel && level != InfoLevel) return true;
            return false;
        }
    }
}