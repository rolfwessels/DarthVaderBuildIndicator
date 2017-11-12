using System;
using BuildIndicatron.Core.Api;
using Microsoft.Extensions.Configuration;

namespace BuildIndicatron.Server.Properties
{
    public class ServerSettings : IJenkinsISettings
    {
        private readonly IConfiguration _configuration;

        private static Lazy<ServerSettings> _instance = new Lazy<ServerSettings>(() => new ServerSettings());

        private ServerSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ServerSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);
            _configuration = builder.Build();
        }
        
        #region singleton

        public static ServerSettings Default => _instance.Value;

        #endregion

        public string SpeachTempFileLocation => _configuration["SpeachTempFileLocation"] ?? "resources/text2speach";
        public string JenkinsHost => _configuration["JenkinsHost"] ?? "http://JenkinsHost:9090";
        public string JenkinsUser => _configuration["JenkinsUser"] ?? "JenkinsUser";
        public string JenkinsPassword => _configuration["JenkinsPassword"] ?? "JenkinsPassword";
        public string SlackKey => _configuration["SlackKey"] ?? "SlackKey";
      
        public String SoundFileLocation => _configuration["SoundFileLocation"] ?? "resources/sounds";


        public static void Initialize(IConfiguration configuration)
        {
            _instance = new Lazy<ServerSettings>(() => new ServerSettings(configuration));
        }
    }
}