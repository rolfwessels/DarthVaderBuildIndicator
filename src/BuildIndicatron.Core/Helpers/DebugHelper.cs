using System;
using Newtonsoft.Json;

namespace BuildIndicatron.Core.Helpers
{
    public static class DebugHelper
    {
        public static String Dump(this object obj, bool format = true)
        {
            return JsonConvert.SerializeObject(obj, format ? Formatting.Indented : Formatting.None);
        }

        public static T Dump<T>(this T obj, string name)
        {
            Console.Out.WriteLine(name + ":" + obj.Dump());
            return obj;
        }

        public static dynamic ToDynamic(this object choreography)
        {
            var serializeObject = JsonConvert.SerializeObject(choreography);
            return JsonConvert.DeserializeObject<dynamic>(serializeObject);
        }

        public static dynamic ToConcrete<T>(this object choreography)
        {
            string serializeObject = JsonConvert.SerializeObject(choreography);
            return JsonConvert.DeserializeObject<T>(serializeObject);
        }
    }
}