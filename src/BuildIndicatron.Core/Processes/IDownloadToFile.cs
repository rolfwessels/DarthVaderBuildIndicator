using System;

namespace BuildIndicatron.Core.Processes
{
    public interface IDownloadToFile
    {
        string DownloadToTempFile(Uri uri, string text);
    }
}