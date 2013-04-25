using System;

namespace BuildIndicatron.App.Core.Log
{
    public static class LogManager
    {
        private static Logger _currentClassLogger;

        public static Logger GetCurrentClassLogger()
        {
            return _currentClassLogger ?? (_currentClassLogger = new LoggerDefault());
        }

        public static ILog GetLogger(Type declaringType)
        {
            return GetCurrentClassLogger();
        }
    }
}