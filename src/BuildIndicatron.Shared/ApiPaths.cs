namespace BuildIndicatron.Shared
{
    public static class ApiPaths
    {
        public static string FileUploadHasFileInArchive = "/fileUpload/{filename}";
        public const string LocalHost = "http://localhost:8080/api/";
        public static string FileUploadUpload = "/fileUpload/";
        public static string Ping = "/ping";
        public const string TextToSpeech = "/TextToSpeech/{text}";
        public const string PlayMp3File = "/playmp3file/{fileName}";
    }
}