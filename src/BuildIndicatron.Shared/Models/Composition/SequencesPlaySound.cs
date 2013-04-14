namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesPlaySound : Sequences
    {
        public SequencesPlaySound() : base("playsound")
        {
        }

        public string File { get; set; }
    }
}