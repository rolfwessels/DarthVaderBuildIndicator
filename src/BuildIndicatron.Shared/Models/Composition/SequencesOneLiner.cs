namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesOneLiner : Sequences
    {
        public SequencesOneLiner()
            : base("oneliner")
        {
        }

        public bool SendTweet { get; set; } 
    }
}