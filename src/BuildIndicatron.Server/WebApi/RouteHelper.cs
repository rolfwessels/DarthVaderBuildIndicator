namespace BuildIndicatron.Server.Api
{
    public class RouteHelper
    {
        public const string ApiPrefix = "api/";
        public const string WithId = "{id}";
        public const string WithDetail = "detail";
        public const string PingController = ApiPrefix + "Ping";
        public const string OutputGpioController = ApiPrefix + "OutputGpio";
        public const string EnqueueController = ApiPrefix + "Enqueue";
        public const string SoundPlayerController = ApiPrefix + " SoundPlayer";
        public const string TextToSpeechController = ApiPrefix + "TextToSpeech";
    }
}