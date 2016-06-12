using System.Threading.Tasks;
using RestSharp;

namespace BuildIndicatron.Core.Helpers
{
    public class HttpLookup : IHttpLookup
    {
        #region Implementation of IHttpLookup

        public async Task<IRestResponse> Download(string baseUrl, int timeout = 5000)
        {
            var restResponse = await new RestClient(baseUrl) { Timeout = timeout }.ExecuteGetTaskAsync(new RestRequest());
            return restResponse;
        }

        #endregion
    }
}