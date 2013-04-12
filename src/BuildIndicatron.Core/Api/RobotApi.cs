using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Shared;
using RestSharp;
using log4net;

namespace BuildIndicatron.Core.Api
{
    public class RobotApi
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RestClient _client;
        private string _applicationJson;

        public RobotApi()
        {
            _client = new RestClient("http://localhost:8888/api");
            _applicationJson = "application/json";
        }

        public Task<HasFileInArchiveResponse> HasFileInArchive(string fileName)
        {
            var restRequest = GetRestRequest(ApiPaths.FileUploadPath, Method.GET);
            return ProcessDefaultRequest<HasFileInArchiveResponse>(restRequest);
        }

        protected RestRequest GetRestRequest(string uri,  Method method , object requestObject = null)
        {
            var request = new RestRequest(uri, method) { RequestFormat = DataFormat.Json };
            
            request.AddHeader("Accept", _applicationJson);
            //add post information
            if (method == Method.POST)
            {
                request.AddHeader("Content-Type", _applicationJson);
                if (requestObject != null)
                {
                    request.AddBody(requestObject);
                }
            }
            return request;
        }


        protected Task<T> ProcessDefaultRequest<T>(IRestRequest request)
            where T :  new()
        {
            var stopwatch = new Stopwatch();
            _log.Debug(string.Format("ApiBase:ProcessDefaultRequest Call to Resource {2} {0}{1} ", _client.BaseUrl, request.Resource, request.Method));
            stopwatch.Start();
            var taskCompletionSource = new TaskCompletionSource<T>();
            _client.ExecuteAsync<T>(request, response =>
                {
                    Exception errorException = null;
                    try
                    {
                        stopwatch.Stop();
                        _log.Debug(string.Format("ApiBase:ProcessDefaultRequest Content {0}{1} [RequestTime:{3}] [{2}]", _client.BaseUrl, request.Resource, response.Content, stopwatch.Elapsed));
                        if (response.ErrorException != null)
                        {
                            throw  response.ErrorException;
                        }
                        var result = response.Data;
                        taskCompletionSource.SetResult(result);
                    }
                    catch (Exception exception)
                    {
                        errorException = exception;
                    
                    }
                    finally
                    {
                        if (errorException != null)
                        {
                            _log.Warn("ApiBase:ProcessDefaultRequest " + errorException.Message, errorException);
                            taskCompletionSource.TrySetException(errorException);
                        }

                    }
                });
            return taskCompletionSource.Task;
        }
    }

   
}