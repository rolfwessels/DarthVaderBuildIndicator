using System;
using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace BuildIndicatron.Core.Helpers
{
    public static class LogHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void ContinuationAction(Task task)
        {
            if (task.Exception != null)
            {
                _log.Error(String.Format("Slackbot error {0}", task.Exception.Message), task.Exception);
            }
        }

        public static void FireAndForgetWithLogging(this Task process)
        {
            process.ContinueWith(ContinuationAction);

        } 
        
        public static void FireAndForgetWithLogging<T>(this Task<T> process)
        {
            process.ContinueWith(ContinuationAction);
        }
    }
}