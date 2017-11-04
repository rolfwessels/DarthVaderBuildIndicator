using System;
using Microsoft.Extensions.Configuration;

namespace BuildIndicatron.Core.Properties
{
    public class Settings
    {
        private static Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings(null));
        private readonly IConfigurationRoot _configuration;

        private Settings(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        
        #region singleton

        public static Settings Default => _instance.Value;

        #endregion

        
        public string ConnectionProxy => _configuration["ConnectionProxy"] ?? "http://192.168.3.6:3128/";


        public static void Initialize(IConfigurationRoot configuration)
        {
            _instance = new Lazy<Settings>(() => new Settings(configuration));
        }
    }
}