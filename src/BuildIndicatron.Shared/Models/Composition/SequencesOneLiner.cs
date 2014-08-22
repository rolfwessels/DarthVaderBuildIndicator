namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesOneLiner : Sequences
    {
	    public const string TypeName = "oneliner";

	    public SequencesOneLiner()
            : base(TypeName)
        {
        }

        public bool SendTweet { get; set; } 
    }
}