using System;
using Microsoft.Extensions.Configuration;

namespace BuildIndicatron.Console
{
    public class AppSettings
    {
        private static Lazy<AppSettings> _instance =
            new Lazy<AppSettings>(() => throw new Exception("Call Initialize before use."));

        private readonly IConfigurationRoot _configuration;

        private AppSettings(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        #region singleton

        public static AppSettings Default => _instance.Value;

        #endregion

        public string JenkenPassword => _configuration["JenkenPassword"] ?? "casd";
        public string JenkenUsername => _configuration["JenkenUsername"] ?? "Rolf";
        public string JenkenServer => _configuration["JenkenServer"] ?? "http://yumi:8080/";

        public string CoreProjects => _configuration["CoreProjects"] ??
                                      "IIAB Integration Tests|IIAB Test|InfoslipWP8 Release|InfoslipWP8 Test";

        public string StringReplaces => _configuration["StringReplaces"] ?? "zapzap:zap zap|";
        public string Host => _configuration["Host"] ?? "http://192.168.40.101:5000/";
        public int PassiveStopHour => Convert.ToInt32(_configuration["PassiveStopHour"] ?? "20");
        public int PassiveStartHour => Convert.ToInt32(_configuration["PassiveStartHour"] ?? "7");
        public int PassiveInterval => Convert.ToInt32(_configuration["PassiveInterval"] ?? "45");
        public int ButtonPin => Convert.ToInt32(_configuration["ButtonPin"] ?? "10");
        public int LsBluePin => Convert.ToInt32(_configuration["LsBluePin"] ?? "11");
        public int LsGreenPin => Convert.ToInt32(_configuration["LsGreenPin"] ?? "9");
        public int LsRedPin => Convert.ToInt32(_configuration["LsRedPin"] ?? "27");
        public int FeetGreenPin => Convert.ToInt32(_configuration["FeetGreenPin"] ?? "24");
        public int FeetRedPin => Convert.ToInt32(_configuration["FeetRedPin"] ?? "17");


        public static void Initialize(IConfigurationRoot configuration)
        {
            _instance = new Lazy<AppSettings>(() => new AppSettings(configuration));
        }
    }
}