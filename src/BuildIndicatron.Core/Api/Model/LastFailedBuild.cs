using System;
using System.Collections.Generic;
using System.Linq;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Api.Model
{
    public class LastFailedBuild
    {
        public int Number { get; set; }
        public string Timestamp { get; set; }
        public ChangeSet ChangeSet { get; set; }
        public DateTime DateTime {
            get
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                double value;
                if (double.TryParse(Timestamp, out value))
                {
                    
                }
                return epoch.AddMilliseconds(value);
            }
        }

        public IEnumerable<string> Authors()
        {
            return ChangeSet != null ? ChangeSet.items.Select(x => x.author.fullName).Distinct() : new string[0];
        }
    }
}