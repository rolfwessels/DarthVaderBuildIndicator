using BuildIndicatron.Shared.Enums;

namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesGpIo : Sequences
    {
        public const string TypeName = "GpIO";

        public SequencesGpIo()
            : base(TypeName)
        {
        }

        public SequencesGpIo(GpIO pin, bool isOn)
            : this()
        {
            Pin = (int) pin;
            IsOn = isOn;
        }

        public SequencesGpIo(PinName pin, bool isOn)
            : this()
        {
            PinName = pin;
            IsOn = isOn;
        }

        public SequencesGpIo(int pin, bool isOn)
            : this()
        {
            Pin = pin;
            IsOn = isOn;
        }

        public int Pin { get; set; }

        public GpIO GpIO()
        {
            return (GpIO) Pin;
        }

        public bool IsOn { get; set; }

        public PinName PinName { get; set; }
    }
}