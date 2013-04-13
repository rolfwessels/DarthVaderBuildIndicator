using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
using RestSharp;

namespace BuildIndicatron.Core.Api
{
    public class RobotApi : ApiBase, IRobotApi
    {
        public RobotApi(string hostApi) : base(hostApi)
        {
        }

        public Task<FileUploadHasFileInArchiveResponse> HasFileInArchive(string inputFile)
        {
            var restRequest = GetRestRequest(ApiPaths.FileUploadHasFileInArchive, Method.GET);
            restRequest.AddUrlSegment("filename", Path.GetFileName(inputFile));
            return ProcessDefaultRequest<FileUploadHasFileInArchiveResponse>(restRequest);
        }

        public Task<FileUploadUploadResponse> UploadFile(string inputFile)
        {
            var restRequest = GetRestRequest(ApiPaths.FileUploadUpload, Method.POST);
            
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            AddFile(inputFile, restRequest);
            return ProcessDefaultRequest<FileUploadUploadResponse>(restRequest);
        }

        public Task<PingResponse> Ping()
        {
            var restRequest = GetRestRequest(ApiPaths.Ping, Method.GET);
            return ProcessDefaultRequest<PingResponse>(restRequest);
        }

        public Task<PlayMp3FileResponse> PlayMp3File(string fileName)
        {
            var restRequest = GetRestRequest(ApiPaths.PlayMp3File, Method.GET);
            restRequest.AddUrlSegment("fileName", fileName);
            return ProcessDefaultRequest<PlayMp3FileResponse>(restRequest);
        }

        public Task<TextToSpeechResponse> TextToSpeech(string text)
        {
            var restRequest = GetRestRequest(ApiPaths.TextToSpeech, Method.GET);
            restRequest.AddUrlSegment("text", text);
            
            return ProcessDefaultRequest<TextToSpeechResponse>(restRequest);
        }

        public Task<SetupGpIoResponse> GpIoSetup(int pin, GPIO direction)
        {
            var restRequest = GetRestRequest(ApiPaths.SetupGpIo, Method.GET);
            restRequest.AddUrlSegment("pin", pin.ToString(CultureInfo.InvariantCulture));
            restRequest.AddUrlSegment("direction", direction.ToString());
            return ProcessDefaultRequest<SetupGpIoResponse>(restRequest);
        }

     
        public Task<GpIoOutputResponse> GpIoOutput(int pin, bool isOn)
        {
            var restRequest = GetRestRequest(ApiPaths.GpIoOutput, Method.GET);
            restRequest.AddUrlSegment("pin", pin.ToString(CultureInfo.InvariantCulture));
            restRequest.AddUrlSegment("ison", isOn.ToString());
            return ProcessDefaultRequest<GpIoOutputResponse>(restRequest);
        }
    }

   
}