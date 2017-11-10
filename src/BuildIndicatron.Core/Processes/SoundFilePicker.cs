using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using log4net;

namespace BuildIndicatron.Core.Processes
{
    public class SoundFilePicker : ISoundFilePicker
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _baseDir;

        public SoundFilePicker(string baseDir)
        {
            _baseDir = Path.GetFullPath(baseDir).AsPath();
            _log.Info(string.Format("Setting sound folder to {0}", _baseDir));
        }

        #region Implementation of ISoundFilePicker

        public IEnumerable<string> GetFolders()
        {
            return Directory.GetDirectories(_baseDir);
        }

        public IEnumerable<string> GetAllSoundFiles(string directory)
        {
            return Directory.GetFiles(directory, "*.mp3").Union(Directory.GetFiles(directory, "*.wav"));
        }

        public string PickFile(string id)
        {
            string fileName = Path.Combine(_baseDir, id);
            _log.Info(string.Format("Trying to play file [{0}]", fileName));
            bool isFile = File.Exists(fileName);
            if (isFile)
            {
                return fileName;
            }
            var folder = Directory.GetDirectories(_baseDir)
                .OrderByDescending(x => (Path.GetFileName(x) ?? "").ToLower() == id.ToLower()).ToList();
            if (folder.Any())
            {
                string[] strings = Directory.GetFiles(folder.First(), "*.*");
                string random = strings.Random();
                return random;
            }
            return null;
        }

        #endregion
    }
}