using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            _dictionary.AddOrUpdate(key, value== null?null: value.Trim());
            Save();
        }

        public string Get(string key, string defaultValue = null)
        {
            if (_dictionary.ContainsKey(key)) 
            return _dictionary[key];
            if (defaultValue != null)
            {
                Set(key, defaultValue);
            }
            return defaultValue;
        }

        public int Get(string buildProcessingTimeout, int defaultValue)
        {
            var stringValue = Get(buildProcessingTimeout,defaultValue.ToString(CultureInfo.InvariantCulture));
            int intValue;
            if (int.TryParse(stringValue, out intValue))
            {
                return intValue;
            }
            return defaultValue;
        }


        public IDictionary<string, string> Get()
        {
            return _dictionary.ToDictionary(x=>x.Key,x=>x.Value);
        }

        #region Private Methods

        private void Save()
        {
            lock (_fileName)
            {
                File.WriteAllText(_fileName, JsonConvert.SerializeObject(_dictionary,Formatting.Indented));
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