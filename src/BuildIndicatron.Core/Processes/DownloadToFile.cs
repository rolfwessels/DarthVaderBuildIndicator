using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using log4net;

namespace BuildIndicatron.Core.Processes
{
    public class DownloadToFile : IDownloadToFile
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISettingsManager _settings;
        private readonly string _tempPath;


        public DownloadToFile(string tempPath, ISettingsManager settings)
        {
            _tempPath = tempPath;
            _settings = settings;
        }

        public string GetFileName(string description)
        {
            if (description == null) throw new ArgumentNullException("description");
            var fileName = Regex.Replace(description, "[^A-z]", "_");
            fileName = fileName.Substring(0, Math.Min(14, fileName.Length));
            return string.Format("{0}.{1}.mp3", fileName, description.Md5Hash());
        }

        #region Implementation of IDownloadToFile

        public string DownloadToTempFile(Uri uri, string text)
        {
            try
            {
                if (!Directory.Exists(_tempPath)) Directory.CreateDirectory(_tempPath);
                var fileName = Path.Combine(_tempPath, GetFileName(text)).AsPath();
                fileName.Dump("fileName");
                
                if (!File.Exists(fileName))
                {
                    uri.Dump("uri");
                    
                    _log.Debug($"DownloadToFile:DownloadToTempFile {uri}");
                    using (var client = new MyWebClient())
                    {
                        if (!string.IsNullOrEmpty(_settings.GetDefaultProxy()))
                        {
                            client.Proxy = new WebProxy(new Uri(_settings.GetDefaultProxy()));
                        }
                        client.DownloadFile(uri, fileName);
                    }
                }
                return fileName;
            }
            catch (Exception e)
            {
                _log.Error("Error downloading from: " + e.Message, e);
                throw;
            }
        }


        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                var request = base.GetWebRequest(uri);
                if (request != null)
                {
                    request.Timeout = 10 * 1000;
                    return request;
                }
                return null;
            }
        }

        #endregion
    }
}