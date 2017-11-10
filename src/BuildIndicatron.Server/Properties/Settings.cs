using System;
using BuildIndicatron.Core.Api;
using Microsoft.Extensions.Configuration;

namespace BuildIndicatron.Server.Properties
{
    public class Settings : IJenkinsISettings
    {
        private readonly IConfigurationRoot _configuration;

        private static Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings());

        private Settings(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        private Settings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);
            _configuration = builder.Build();
        }
        
        #region singleton

        public static Settings Default => _instance.Value;

        #endregion

        public string SpeachTempFileLocation => _configuration["SpeachTempFileLocation"] ?? "resources/text2speach";
        public string JenkinsHost => _configuration["JenkinsHost"] ?? "http://JenkinsHost:9090";
        public string JenkinsUser => _configuration["JenkinsUser"] ?? "JenkinsUser";
        public string JenkinsPassword => _configuration["JenkinsPassword"] ?? "JenkinsPassword";
        public string SlackKey => _configuration["SlackKey"] ?? "SlackKey";
       
        public String SoundFileLocation => _configuration["SoundFileLocation"] ?? "resources/sounds";


        public static void Initialize(IConfigurationRoot configuration)
        {
            _instance = new Lazy<Settings>(() => new Settings(configuration));
        }
    }
}