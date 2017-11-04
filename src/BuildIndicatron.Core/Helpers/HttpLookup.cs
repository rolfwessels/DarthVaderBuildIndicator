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

        public Task<IRestResponse> Download(string baseUrl, int timeout = 10000)
        {
            var restClient = new RestClient(baseUrl) { Timeout = timeout };
//            if (!string.IsNullOrEmpty(_settings.GetDefaultProxy()))
//            {
//                restClient.Proxy = new WebProxy(new Uri(_settings.GetDefaultProxy()));
//            }
            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();

            restClient.ExecuteAsyncGet(new RestRequest(), (response, handle) =>
            {
                if (response.ErrorException != null)
                {
                    taskCompletionSource.SetException(response.ErrorException);
                }
                else
                {
                    taskCompletionSource.SetResult(response);
                }
            },"GET");

            
            return taskCompletionSource.Task;
        }

        #endregion
    }
}