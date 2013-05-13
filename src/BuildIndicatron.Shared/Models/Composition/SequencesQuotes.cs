namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesQuotes : Sequences
    {
        public SequencesQuotes()
            : base("Quotes")
        {
        }

        public bool TweetIt { get; set; } 
    }
}