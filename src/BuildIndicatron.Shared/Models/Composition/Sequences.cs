namespace BuildIndicatron.Shared.Models.Composition
{
    public class Sequences
    {
        private readonly string _type;

        public Sequences(string type)
        {
            _type = type;
        }

        public string Type
        {
            get { return _type; }
        }

        public int BeginTime { get; set; }
    }
}