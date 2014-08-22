using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Processes
{
	public class DownloadToFile : IDownloadToFile
	{
		private string _tempPath;

		public DownloadToFile(string tempPath)
		{
			_tempPath = tempPath;
		}

		public string GetFileName(string description)
		{
			if (description == null) throw new ArgumentNullException("description");
			var fileName = Regex.Replace(description, "[^A-z]", "_");
			fileName = fileName.Substring(0, Math.Min(14, fileName.Length));
			var hashCode = Math.Abs(description.GetHashCode());
			return string.Format("{0}.{1}.mp3", fileName, hashCode.ToString());
		}

		#region Implementation of IDownloadToFile

		public string DownloadToTempFile(Uri uri, string text)
		{
			var fileName = Path.Combine(_tempPath, GetFileName(text)).AsPath();
			if (!File.Exists(fileName))
			{
				using (var client = new WebClient())
				{
					client.DownloadFile(uri, fileName);
				}
			}
			return fileName;
		}

		

		#endregion
	}
}