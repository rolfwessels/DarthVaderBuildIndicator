namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesInsult : Sequences
    {
        public const string TypeName = "Insult";

        public SequencesInsult()
            : base(TypeName)
        {
        }

        public bool SendTweet { get; set; }
    }
}