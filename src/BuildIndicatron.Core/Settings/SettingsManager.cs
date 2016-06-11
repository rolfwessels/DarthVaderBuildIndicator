using System;
using System.Collections.Generic;
using System.IO;
using BuildIndicatron.Core.Helpers;
using Newtonsoft.Json;

namespace BuildIndicatron.Core.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly string _fileName;
        private Dictionary<string, string> _dictionary;

        public SettingsManager(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            _fileName = fileName;
            Load();
        }

        public void Set(string key, string value)
        {
            _dictionary.AddOrUpdate(key, value);
            Save();
        }

        public string Get(string key, string defaultValue = null)
        {
            return _dictionary.ContainsKey(key) ? _dictionary[key] : defaultValue;
        }

        #region Private Methods

        private void Save()
        {
            lock (_fileName)
            {
                File.WriteAllText(_fileName, JsonConvert.SerializeObject(_dictionary));
            }
        }

        private void Load()
        {
            lock (_fileName)
            {
                if (File.Exists(_fileName))
                {
                    string readAllText = File.ReadAllText(_fileName);
                    _dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(readAllText);
                }
                _dictionary = _dictionary ?? new Dictionary<string, string>();
            }
        }

        #endregion
    }
}