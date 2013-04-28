using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using RestSharp;

namespace BuildIndicatron.Core.Api
{
    public class RobotApi : ApiBase, IRobotApi
    {
        public RobotApi(string hostApi) : base(hostApi)
        {
        }

        #region IRobotApi Members

        public Task<FileUploadHasFileInArchiveResponse> HasFileInArchive(string inputFile)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.FileUploadHasFileInArchive, Method.GET);
            restRequest.AddUrlSegment("filename", Path.GetFileName(inputFile));
            return ProcessDefaultRequest<FileUploadHasFileInArchiveResponse>(restRequest);
        }

        public Task<FileUploadUploadResponse> UploadFile(string inputFile)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.FileUploadUpload, Method.POST);

            restRequest.AddHeader("Content-Type", "multipart/form-data");
            AddFile(inputFile, restRequest);
            return ProcessDefaultRequest<FileUploadUploadResponse>(restRequest);
        }

        public Task<PingResponse> Ping()
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.Ping, Method.GET);
            return ProcessDefaultRequest<PingResponse>(restRequest);
        }

        public Task<PlayMp3FileResponse> PlayMp3File(string fileName)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.PlayMp3File, Method.GET);
            restRequest.AddUrlSegment("fileName", fileName);
            return ProcessDefaultRequest<PlayMp3FileResponse>(restRequest);
        }

        public Task<TextToSpeechResponse> TextToSpeech(string text)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.TextToSpeech, Method.GET);
            restRequest.AddUrlSegment("text", text);

            return ProcessDefaultRequest<TextToSpeechResponse>(restRequest);
        }

        public Task<SetupGpIoResponse> GpIoSetup(int pin, Gpio direction)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.SetupGpIo, Method.GET);
            restRequest.AddUrlSegment("pin", pin.ToString(CultureInfo.InvariantCulture));
            restRequest.AddUrlSegment("direction", direction.ToString());
            return ProcessDefaultRequest<SetupGpIoResponse>(restRequest);
        }

        public Task<GpIoOutputResponse> GpIoOutput(int pin, bool isOn)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.GpIoOutput, Method.GET);
            restRequest.AddUrlSegment("pin", pin.ToString(CultureInfo.InvariantCulture));
            restRequest.AddUrlSegment("ison", isOn.ToString());
            return ProcessDefaultRequest<GpIoOutputResponse>(restRequest);
        }

        public Task<PassiveProcessResponse> PassiveProcess()
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.PassiveProcess, Method.GET);
            return ProcessDefaultRequest<PassiveProcessResponse>(restRequest);
        }

        public Task<PassiveProcessResponse> PassiveProcess(Passive passive)
        {
            RestRequest restRequest = GetRestRequest(ApiPaths.PassiveProcess, Method.POST, passive);
            return ProcessDefaultRequest<PassiveProcessResponse>(restRequest);
        }

        public Task<EnqueueResponse> Enqueue(Choreography choreography)
        {
            var restRequest = GetRestRequest(ApiPaths.Enqueue, Method.POST, choreography);
            return ProcessDefaultRequest<EnqueueResponse>(restRequest);
        }

        public Task<SetButtonChoreographyResponse> SetButtonChoreography(params Choreography[] choreography)
        {
            var restRequest = GetRestRequest(ApiPaths.SetButtonChoreography, Method.POST, choreography);
            return ProcessDefaultRequest<SetButtonChoreographyResponse>(restRequest);
        }

        public Task<GetClipsResponse> GetClips()
        {
            var restRequest = GetRestRequest(ApiPaths.GetClips, Method.GET);
            return ProcessDefaultRequest<GetClipsResponse>(restRequest);
        }


        #endregion
    }
}