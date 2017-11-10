namespace BuildIndicatron.Core.Api
{
    public interface IJenkinsISettings
    {
        string JenkinsHost { get; }
        string JenkinsUser { get; }
        string JenkinsPassword { get; }
    }
}