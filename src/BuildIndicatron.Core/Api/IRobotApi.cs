using System.Threading.Tasks;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Core.Api
{
    public interface IRobotApi
    {
        Task<FileUploadHasFileInArchiveResponse> HasFileInArchive(string inputFile);
        Task<FileUploadUploadResponse> UploadFile(string inputFile);
        Task<PingResponse> Ping();
        Task<PlayMp3FileResponse> PlayMp3File(string fileName);
        Task<TextToSpeechResponse> TextToSpeech(string text);
        Task<SetupGpIoResponse> GpIoSetup(int pin, Gpio direction);
        Task<GpIoOutputResponse> GpIoOutput(int pin, bool isOn);
        Task<PassiveProcessResponse> PassiveProcess();
        Task<PassiveProcessResponse> PassiveProcess(Passive passive);
        Task<EnqueueResponse> Enqueue(Choreography choreography);
        Task<SetButtonChoreographyResponse> SetButtonChoreography(Choreography choreography);




       
    }

   
}