using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using RestSharp;
using RestSharp.Authenticators;
#if WINDOWS_PHONE
using BuildIndicatron.App.Core.Log;
using BuildIndicatron.App.Core.Task;
#else
using log4net;
using System.Threading.Tasks;

#endif

namespace BuildIndicatron.Core.Api
{
    public class ApiBase
    {
        private const string ApplicationJson = "application/json";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly RestClient Client;

        public ApiBase(string hostApi, string username = null, string password = null)
        {
            Client = new RestClient(hostApi);
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                Client.Authenticator = new HttpBasicAuthenticator(username, password);
        }


        protected RestRequest GetRestRequest(string uri, Method method, object requestObject = null)
        {
            var request = new RestRequest(uri, method) {RequestFormat = DataFormat.Json};

            request.AddHeader("Accept", ApplicationJson);
            //add post information
            if (method == Method.POST)
            {
                if (requestObject != null)
                {
                    request.AddHeader("Content-Type", ApplicationJson);
                    request.AddBody(requestObject);
                }
            }
            return request;
        }


        protected Task<T> ProcessDefaultRequest<T>(IRestRequest request)
            where T : new()
        {
            var stopwatch = new Stopwatch();
            Uri buildUri = Client.BuildUri(request);
            _log.Debug(string.Format("ApiBase:ProcessDefaultRequest {0} {1} [{2}]", request.Method, buildUri,
                request.Parameters.FirstOrDefault(x => x.Name == ApplicationJson)));
            stopwatch.Start();
            var taskCompletionSource = new TaskCompletionSource<T>();

            Client.ExecuteAsync<T>(request, response =>
            {
                Exception errorException = null;
                try
                {
                    stopwatch.Stop();
                    _log.Debug(string.Format("ApiBase:ProcessDefaultRequest Content {0} [RequestTime:{1}] [{2}]",
                        buildUri, stopwatch.ElapsedMilliseconds, response.Content));
                    if (response.ErrorException == null && response.StatusCode == HttpStatusCode.OK)
                    {
                        T result = response.Data;
                        taskCompletionSource.SetResult(result);
                    }
                    else
                    {
                        if (response.ErrorException != null) errorException = response.ErrorException;
                        else if (!string.IsNullOrEmpty(response.ErrorMessage))
                            throw new Exception(response.ErrorMessage);
                        else
                            throw new Exception(string.Format("Status code returned = [{0} {1}]",
                                (int) response.StatusCode, response.StatusCode));
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
                        _log.Warn("ApiBase:ProcessDefaultRequest " + errorException.Message, errorException);
                        taskCompletionSource.TrySetException(errorException);
                    }
                }
            });
            return taskCompletionSource.Task;
        }

        protected void AddFile(string inputFile, RestRequest restRequest)
        {
            restRequest.AddFile(Path.GetFileNameWithoutExtension(inputFile), ReadToEnd(inputFile),
                Path.GetFileName(inputFile), MimeHelper.GetMimeType(Path.GetExtension(inputFile)));
        }

        #region Private Methods

        private byte[] ReadToEnd(string fileName)
        {
            using (FileStream stream = File.OpenRead(fileName))
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
                            var temp = new byte[readBuffer.Length*2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);
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

        #endregion
    }
}