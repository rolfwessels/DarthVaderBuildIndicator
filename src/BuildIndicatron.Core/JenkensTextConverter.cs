using System;
using System.Collections.Generic;
using System.Linq;
using BuildIndicatron.Core.Api.Model;

namespace BuildIndicatron.Core
{
    public class JenkensTextConverter
    {
        private const string FailColor = "red";
        private const string SuccessColor = "blue";

        public string ToSummary(JenkensProjectsResult jenkensProjectsResult)
        {
            var strings = ToSummaryList(jenkensProjectsResult).ToArray();
            if (strings.Length > 0)
            {
                return string.Join(". ", strings);
            }
            return "No jenkins data received. Please try again later!";
        }

        public IEnumerable<string> ToSummaryList(JenkensProjectsResult jenkensProjectsResult)
        {
            if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.All(x => x.Color == SuccessColor))
            {
                yield return string.Format("Yea, there are currently {0} {1} on jenkins and they are all passing",                                      jenkensProjectsResult.Jobs.Count,
                                     jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds");
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.All(x => x.Color == FailColor))
            {
                yield return string.Format("Oh no, there are currently {0} {1} on jenkins and they are all broken. Maybe development is not for you",
                                     jenkensProjectsResult.Jobs.Count,
                                     jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds");
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.Any())
            {
                var failedValues = jenkensProjectsResult.Jobs.Where(x => x.Color == FailColor).ToList();
                

                yield return string.Format("Oh no, there are currently {0} {2} on jenkins with {1} {3} failing",
                                     jenkensProjectsResult.Jobs.Count,
                                     failedValues.Count,
                                     jenkensProjectsResult.Jobs.Count == 1? "build":"builds",
                                     failedValues.Count == 1? "build":"builds"
                                     ) ;
                var failedBuildDetail = FailedBuildDetail(failedValues).ToArray();
                foreach (var failedValue in failedBuildDetail)
                {
                    yield return failedValue;
                }
            }
            
        }

        private IEnumerable<string> FailedBuildDetail(IEnumerable<Job> failedValues)
        {
            foreach (var job in failedValues.Where(x=>x.LastFailedBuild != null ))
            {
                var names = job.LastFailedBuild.ChangeSet.items.Select(x => x.author.fullName).Distinct().ToArray();
                if (names.Length == 0) names = new[] {"a ghost"};
                yield return string.Format("The {0} last failed {1}, It was last modified by {2}", 
                                           job.Name , 
                                           GetLastModifiedDateString(job.LastFailedBuild),
                                           String.Join(" and ",names) );
            }
        }

        public string GetLastModifiedDateString(LastFailedBuild lastFailedBuild)
        {
            var timeSpan = DateTime.Now - lastFailedBuild.DateTime.AddSeconds(-1);
            return timeSpan.TotalDays > 2000 ? "uhhmm who knows" : HumanReadable(timeSpan);
        }

        public static string HumanReadable(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays > 60)
            {
                return ((int)(timeSpan.TotalDays / 30)) + " months ago";
            }
            if (timeSpan.TotalDays > 30)
            {
                return ((int)(timeSpan.TotalDays / 30)) + " month ago";
            }
            if (timeSpan.TotalDays > 2)
            {
                return ((int)timeSpan.TotalDays) + " days ago";
            }
            if (timeSpan.TotalHours > 23)
            {
                return ((int)timeSpan.TotalDays) + " day ago";
            }
            if (timeSpan.TotalMinutes > 110)
            {
                return ((int)timeSpan.TotalHours) + " hours ago";
            }
            if (timeSpan.TotalMinutes > 60)
            {
                return ((int)timeSpan.TotalHours) + " hour ago";
            }
            if (timeSpan.TotalMinutes > 2)
            {
                return ((int)timeSpan.TotalMinutes) + " minutes ago";
            }
            if (timeSpan.TotalSeconds > 59)
            {
                return ((int)timeSpan.TotalMinutes) + " minute ago";
            }
            if (timeSpan.TotalSeconds > 5)
            {
                return ((int)timeSpan.TotalSeconds) + " seconds ago";
            }
            return "just now";
        }

        
    }
}