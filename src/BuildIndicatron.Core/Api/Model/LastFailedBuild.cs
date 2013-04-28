using System;

namespace BuildIndicatron.Core.Api.Model
{
    public class LastFailedBuild
    {
        public int Number { get; set; }
        public string Timestamp { get; set; }
        public ChangeSet ChangeSet { get; set; }
        public DateTime DateTime {
            get {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return epoch.AddMilliseconds(Convert.ToDouble(Timestamp));
            }
        }
    }
}