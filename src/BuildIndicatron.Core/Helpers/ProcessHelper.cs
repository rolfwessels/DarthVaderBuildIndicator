using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace BuildIndicatron.Core.Helpers
{
	public class ProcessHelper 
	{
	  private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public static void Run(string fileName, string arguments)
		{
			var startInfo = new ProcessStartInfo(fileName)
				{
					WindowStyle = ProcessWindowStyle.Minimized,
					Arguments = arguments,
					RedirectStandardError = true,
					UseShellExecute = false,
          RedirectStandardOutput = true
				};
			var process = Process.Start(startInfo);
      string output = process.StandardOutput.ReadToEnd();
      string err = process.StandardError.ReadToEnd();
      if (!string.IsNullOrEmpty(output))
        _log.Debug(string.Format("ProcessHelper:Run output:{0}", output));
      if (!string.IsNullOrEmpty(err))
        _log.Debug(string.Format("ProcessHelper:Run error:{0}", err));
			process.WaitForExit(30000);
		}

		public static void Run(string fileName)
		{
			var strings = fileName.Split('|');
			Run(strings.FirstOrDefault(), strings.Skip(1).StringJoin("|"));
		}
	}
}