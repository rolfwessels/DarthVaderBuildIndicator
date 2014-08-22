using System;
using Newtonsoft.Json;

namespace BuildIndicatron.Core.Helpers
{
	public static class DebugHelper
	{
		public static String Dump(this object choreography)
		{
			return JsonConvert.SerializeObject(choreography, Formatting.Indented);
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