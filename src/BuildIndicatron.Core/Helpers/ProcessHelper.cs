using System.Diagnostics;
using System.Linq;

namespace BuildIndicatron.Core.Helpers
{
	public class ProcessHelper
	{
		public static void Run(string fileName, string arguments)
		{
			var startInfo = new ProcessStartInfo(fileName)
				{
					WindowStyle = ProcessWindowStyle.Minimized,
					Arguments = arguments,
					RedirectStandardError = true,
					UseShellExecute = false
				};
			var process = Process.Start(startInfo);
			process.WaitForExit(30000);
		}

		public static void Run(string fileName)
		{
			var strings = fileName.Split('|');
			Run(strings.FirstOrDefault(), strings.Skip(1).StringJoin("|"));
		}
	}
}