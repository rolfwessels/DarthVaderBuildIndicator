namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesGpIo : Sequences
    {
        public SequencesGpIo()
            : base("GpIO")
        {
        }

        public SequencesGpIo(int pin, bool isOn) : this()
        {
            Pin = pin;
            IsOn = isOn;
        }

        public int Pin { get; set; }

        public bool IsOn { get; set; }
    }
}