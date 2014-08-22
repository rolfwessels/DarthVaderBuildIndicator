namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesPlaySound : Sequences
    {
	    public const string TypeName = "playsound";

	    public SequencesPlaySound() : base(TypeName)
        {
        }

        public string File { get; set; }
    }
}