using System.Threading.Tasks;
using BuildIndicatron.Shared;
using BuildIndicatron.Shared.Models;

namespace BuildIndicatron.Core.Api
{
    public interface IRobotApi
    {
        Task<FileUploadHasFileInArchiveResponse> HasFileInArchive(string inputFile);
        Task<FileUploadUploadResponse> UploadFile(string inputFile);
        Task<PingResponse> Ping();
        Task<PlayMp3FileResponse> PlayMp3File(string fileName);
        Task<TextToSpeechResponse> TextToSpeech(string text);
    }

    
}