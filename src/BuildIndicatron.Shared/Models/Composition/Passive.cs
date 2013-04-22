using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.Composition
{
    public class Passive
    {
        public int Interval { get; set; }

        public int StartTime { get; set; }

        public int SleepTime { get; set; }

        public List<Choreography> Compositions { get; set; }
    }
}