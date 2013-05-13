namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesTweet : Sequences
    {
        public SequencesTweet()
            : base("Tweet")
        {
        }

        public string Text { get; set; }
    }
}