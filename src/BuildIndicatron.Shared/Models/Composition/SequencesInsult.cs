namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesInsult : Sequences
    {
        public SequencesInsult()
            : base("Insult")
        {
        }

        public bool SendTweet { get; set; } 
    }
}