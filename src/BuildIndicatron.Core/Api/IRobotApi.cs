using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
#if WINDOWS_PHONE
using BuildIndicatron.App.Core.Task;
#else
using log4net;
using System.Threading.Tasks;
#endif

namespace BuildIndicatron.Core.Api
{
    public interface IRobotApi
    {
        Task<FileUploadHasFileInArchiveResponse> HasFileInArchive(string inputFile);
        Task<FileUploadUploadResponse> UploadFile(string inputFile);
        Task<PingResponse> Ping();
        Task<PlayMp3FileResponse> PlayMp3File(string fileName);
        Task<TextToSpeechResponse> TextToSpeech(string text);
        Task<SetupGpIoResponse> GpIoSetup(int pin, GpioDirection direction);
        Task<GpIoOutputResponse> GpIoOutput(int pin, bool isOn);
        Task<PassiveProcessResponse> PassiveProcess();
        Task<PassiveProcessResponse> PassiveProcess(Passive passive);
        Task<EnqueueResponse> Enqueue(Choreography choreography);
        Task<SetButtonChoreographyResponse> SetButtonChoreography(params Choreography[] choreography);
        Task<GetClipsResponse> GetClips();
    }

   
}

