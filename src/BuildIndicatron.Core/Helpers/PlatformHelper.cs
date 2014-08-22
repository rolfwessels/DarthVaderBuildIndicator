using System;

namespace BuildIndicatron.Core.Helpers
{
	public static class PlatformHelper
	{
		public static string AsPath(this string getFullPath)
		{
			return IsLinux ? getFullPath.Replace("\\", "/") : getFullPath.Replace("/", "\\");
		}

		public static string CurrentPlatform
		{
			get { return Environment.OSVersion.Platform.ToString(); }
			
		}

		public static bool IsLinux
		{
			get { return Environment.OSVersion.Platform == PlatformID.Unix; }
			
		}
	}
}