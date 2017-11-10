namespace BuildIndicatron.Shared
{
    public static class ApiPaths
    {
        public static string FileUploadHasFileInArchive = "/fileUpload/{filename}";
        public const string LocalHost = "http://localhost:8080/api/";
        public static string FileUploadUpload = "/fileUpload/";
        public static string Ping = "/ping";
        public const string TextToSpeech = "/texttospeech/{text}";
        public const string TextToSpeechEnhanceSpeech = "/texttospeech/{text}/EnhanceSpeech";
        public const string PlayMp3File = "/SoundPlayer/{fileName}";
        public const string GetClips = "/SoundPlayer";
        public const string SetupGpIo = "/setupgpio/{pin}/{direction}";
        public const string GpIoOutput = "/ouputgpio/{pin}/{ison}";
        public const string PassiveProcess = "/passive";
        public const string Enqueue = "/enqueue";
        public const string SetButtonChoreography = "/setButtonChoreography";
    }
}