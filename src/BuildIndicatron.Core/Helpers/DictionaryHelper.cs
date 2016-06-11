using System.Collections.Generic;

namespace BuildIndicatron.Core.Helpers
{
    static internal class DictionaryHelper
    {
        public static void AddOrUpdate(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }
    }
}