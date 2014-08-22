namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesGpIo : Sequences
    {
	    public const string TypeName = "GpIO";

	    public SequencesGpIo()
            : base(TypeName)
        {
        }

        public SequencesGpIo(int pin, bool isOn) : this()
        {
            Pin = pin;
            IsOn = isOn;
        }

        public int Pin { get; set; }

        public bool IsOn { get; set; }

		public Pins Target { get; set; }

	    public enum Pins
	    {
		    MainLightGreen,
		    MainLightRed,
		    MainLightBlue,
		    SecondaryLightGreen,
			SecondaryLightRed,
			SecondaryLightBlue,
			Spinner,
			Fire,
	    }
    }
}