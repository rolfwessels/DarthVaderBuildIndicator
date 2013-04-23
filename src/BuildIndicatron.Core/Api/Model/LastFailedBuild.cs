namespace BuildIndicatron.Core.Api.Model
{
    public class LastFailedBuild
    {
        public int number { get; set; }
        public string timestamp { get; set; }
        public ChangeSet changeSet { get; set; }
    }
}