using System.Collections.Generic;

namespace BuildIndicatron.Shared.Models.Composition
{
    public class Passive
    {
        public int Interval { get; set; }

        public string StartTime { get; set; }

        public string SleepTime { get; set; }

        public List<Choreography> Compositions { get; set; }
    }
}