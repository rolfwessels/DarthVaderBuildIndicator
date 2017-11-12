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

        #region IHttpLookup Members

        #region Implementation of IHttpLookup

        public Task<IRestResponse> Download(string baseUrl, int timeout = 10000)
        {
            var restClient = new RestClient(baseUrl) {Timeout = timeout};
//            if (!string.IsNullOrEmpty(_settings.GetDefaultProxy()))
//            {
//                restClient.Proxy = new WebProxy(new Uri(_settings.GetDefaultProxy()));
//            }
            return restClient.ExecAsync(new RestRequest(), "GET");
        }

        #endregion

        #endregion
    }

    public static class RestSharpHelper
    {
        public static Task<IRestResponse> ExecAsyncGet(this RestClient restClient, RestRequest restRequest)
        {
            return ExecAsync(restClient, restRequest, "Get");
        }

        public static Task<IRestResponse> ExecAsync(this RestClient restClient, RestRequest restRequest,
            string httpMethod)
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();

            restClient.ExecuteAsyncGet(restRequest, (response, handle) =>
            {
                if (response.ErrorException != null)
                    taskCompletionSource.SetException(response.ErrorException);
                else
                    taskCompletionSource.SetResult(response);
            }, httpMethod);


            return taskCompletionSource.Task;
        }
    }
}