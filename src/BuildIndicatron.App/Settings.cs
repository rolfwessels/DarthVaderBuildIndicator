using System.IO.IsolatedStorage;

namespace BuildIndicatron.App
{
    /// <summary>
    /// </summary>
    class Settings
    {

        private static Settings _instance = null;
        private static readonly object Locker = new object();

        private Settings()
        {
            var isolatedStorageSettings = IsolatedStorageSettings();
            string value;
            if (isolatedStorageSettings.TryGetValue("port", out value))
            {
                Port = value;
            }
            else
            {
                Port = "5000";
            }
            if (isolatedStorageSettings.TryGetValue("host", out value))
            {
                Host = value;
            }
            else
            {
                Host = "192.168.1.13";
            }
        }

        #region singleton

        public static Settings Instance
        {
            get
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new Settings();
                    }
                }
                return _instance;
            }

        }

        public string Host { get; set; }

        public string Port { get; set; }

        #endregion

        public void Save()
        {
            var isolatedStorageSettings = IsolatedStorageSettings();
            isolatedStorageSettings["port"] = Port;
            isolatedStorageSettings["host"] = Host;
            isolatedStorageSettings.Save();
        }

        private static IsolatedStorageSettings IsolatedStorageSettings()
        {
            return System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
        }
    }
}