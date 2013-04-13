using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using RestSharp;
using log4net;

namespace BuildIndicatron.Core.Api
{
    public class ApiBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RestClient _client;
        private readonly string _applicationJson = "application/json";

        public ApiBase(string hostApi)
        {
            _client = new RestClient(hostApi);
        }


        protected RestRequest GetRestRequest(string uri,  Method method , object requestObject = null)
        {
            var request = new RestRequest(uri, method) { RequestFormat = DataFormat.Json };
            request.AddHeader("Accept", _applicationJson);
            //add post information
            if (method == Method.POST)
            {
                if (requestObject != null)
                {
                    request.AddHeader("Content-Type", _applicationJson);
                    request.AddBody(requestObject);
                }
            }
            return request;
            
        }

        

        protected Task<T> ProcessDefaultRequest<T>(IRestRequest request)
            where T :  new()
        {
            var stopwatch = new Stopwatch();
            var buildUri = _client.BuildUri(request);
            Log.Debug(string.Format("ApiBase:ProcessDefaultRequest {0} {1} [{2}]", request.Method, buildUri, request.Parameters.FirstOrDefault(x=>x.Name == _applicationJson)));
            stopwatch.Start();
            var taskCompletionSource = new TaskCompletionSource<T>();
            RestClientExtensions.ExecuteAsync<T>(_client, request, response =>
                {
                    Exception errorException = null;
                    try
                    {
                        stopwatch.Stop();
                        Log.Debug(string.Format("ApiBase:ProcessDefaultRequest Content {0} [RequestTime:{1}] [{2}]", buildUri, stopwatch.Elapsed, response.Content));
                        if (response.ErrorException == null && response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = response.Data;
                            taskCompletionSource.SetResult(result);
                        }
                        else
                        {
                            if (response.ErrorException != null) errorException = response.ErrorException;
                            else if (!string.IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);
                            else
                                throw new Exception(string.Format("Status code returned = [{0} {1}]", (int) response.StatusCode, response.StatusCode));
                        }
                    }
                    catch (Exception exception)
                    {
                        errorException = exception;
                    
                    }
                    finally
                    {
                        if (errorException != null)
                        {
                            Log.Warn("ApiBase:ProcessDefaultRequest " + errorException.Message, errorException);
                            taskCompletionSource.TrySetException(errorException);
                        }
                    }
                });
            return taskCompletionSource.Task;
        }

        protected void AddFile(string inputFile, RestRequest restRequest)
        {
            restRequest.AddFile(Path.GetFileNameWithoutExtension(inputFile), (byte[]) ReadToEnd(inputFile), Path.GetFileName(inputFile), MimeHelper.GetMimeType(Path.GetExtension(inputFile)));
        }

        private byte[] ReadToEnd(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                var readBuffer = new byte[4096];
                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            var temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
        }
    }
}