using System.Collections.Generic;

namespace BuildIndicatron.Core.Api.Model
{
    public class Job
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<Health> HealthReport { get; set; }
        public LastFailedBuild LastFailedBuild { get; set; }
    }

    public class Health
    {
        public int Score { get; set; }
    }
}