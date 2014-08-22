using System;

namespace BuildIndicatron.Shared.Models.ApiResponses
{
    public class PingResponse : BaseResponse
    {
	    public string Version { get; set; }

		public string Platform { get; set; }
    }
}