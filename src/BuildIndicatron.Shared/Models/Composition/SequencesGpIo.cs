namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesGpIo : Sequences
    {
        public SequencesGpIo()
            : base("GpIO")
        {
        }

        public int Pin { get; set; }

        public bool IsOn { get; set; }
    }

    
}