using System.Collections.Generic;

namespace BuildIndicatron.Core.Settings
{
    public interface ISettingsManager
    {
        void Set(string key, string value);
        string Get(string key, string defaultValue = null);
        IDictionary<string, string> Get();
        int Get(string buildProcessingTimeout, int defaultValue);
    }
}