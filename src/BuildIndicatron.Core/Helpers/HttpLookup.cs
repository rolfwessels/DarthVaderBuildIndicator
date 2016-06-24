using System;
using System.Net;
using System.Threading.Tasks;
using BuildIndicatron.Core.Settings;
using RestSharp;

namespace BuildIndicatron.Core.Helpers
{
    public class HttpLookup : IHttpLookup
    {
        private readonly ISettingsManager _settings;

        public HttpLookup(ISettingsManager settings)
        {
            _settings = settings;
        }

        #region Implementation of IHttpLookup

        public async Task<IRestResponse> Download(string baseUrl, int timeout = 10000)
        {
            var restClient = new RestClient(baseUrl) { Timeout = timeout };
            if (!string.IsNullOrEmpty(_settings.GetDefaultProxy()))
            {
                restClient.Proxy = new WebProxy(new Uri(_settings.GetDefaultProxy()));
            }
            var restResponse = await restClient.ExecuteGetTaskAsync(new RestRequest());
            
            return restResponse;
        }

        #endregion
    }
}