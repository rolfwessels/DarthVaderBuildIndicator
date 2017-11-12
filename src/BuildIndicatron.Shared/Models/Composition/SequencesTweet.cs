namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesTweet : Sequences
    {
        public const string TypeName = "Tweet";

        public SequencesTweet()
            : base(TypeName)
        {
        }

        public string Text { get; set; }
    }
}