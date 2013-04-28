namespace BuildIndicatron.Shared.Models.Composition
{
    public class SequencesText2Speech : Sequences
    {
        public SequencesText2Speech() : base("Text2Speech")
        {
        }

        public string Text { get; set; }

        public bool DisableTransform { get; set; }
    }
}