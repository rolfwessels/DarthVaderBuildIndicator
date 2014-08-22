namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesQuotes : Sequences
    {
	    public const string TypeName = "Quotes";

	    public SequencesQuotes()
            : base(TypeName)
        {
        }

        public bool SendTweet { get; set; } 
    }
}