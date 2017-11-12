using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace BuildIndicatron.Core.Helpers
{
    public static class TaskHelper
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogExceptions(Task run,string message = "Fire and forget")
        {
            run.ContinueWith(task =>
            {
                if (run.Exception != null) _log.Error(message+" "+run.Exception.Message, run.Exception);
            });
        }
    }
}