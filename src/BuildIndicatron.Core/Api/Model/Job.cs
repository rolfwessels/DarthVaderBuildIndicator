using System.Collections.Generic;

namespace BuildIndicatron.Core.Api.Model
{
    public class Job
    {
        public string name { get; set; }
        public string color { get; set; }
        public List<Health> healthReport { get; set; }
        public LastFailedBuild lastFailedBuild { get; set; }
    }

    public class Health
    {
        public int score { get; set; }
    }
}