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
                yield return string.Format("Good work, there are currently {0} {1} on jenkins and they are all passing",                                      jenkensProjectsResult.Jobs.Count,
                                     jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds");
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.All(x => x.Color == FailColor))
            {
                yield return string.Format("There are currently {0} {1} on jenkins and they are all broken. Maybe development is not for you",
                                     jenkensProjectsResult.Jobs.Count,
                                     jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds");
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.Any())
            {
                var failedValues = jenkensProjectsResult.Jobs.Where(x => x.Color == FailColor).ToList();


                yield return string.Format("You have failed me, there are currently {0} {2} on jenkins with {1} {3} failing",
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
            var slowestAndFastedBuild = GetSlowestAndFastedBuild(jenkensProjectsResult);
            if (slowestAndFastedBuild != null) yield return slowestAndFastedBuild;

            var mostSuccessFullBuild = GetMostSuccessFullBuild(jenkensProjectsResult);
            if (mostSuccessFullBuild != null) yield return mostSuccessFullBuild;
        }

        private string GetMostSuccessFullBuild(JenkensProjectsResult jenkensProjectsResult)
        {
            if (jenkensProjectsResult.Jobs != null)
            {
                var orderedEnumerable =
                    jenkensProjectsResult.Jobs.Where(x => x.Builds != null && x.Builds.Count > 3)
                                         .Select(x => new { x.Name, SuccessFullBuilds = SuccessFullBuildInARow(x.Builds) })
                                         .OrderByDescending(x => x.SuccessFullBuilds)
                                         .ToArray();

                if (orderedEnumerable.Length > 2)
                {
                    return string.Format("{0} has {1} succesfull builds in a row. {2} has {3} succesfull builds in a row",
                                         orderedEnumerable.First().Name,
                                         orderedEnumerable.First().SuccessFullBuilds,
                                         orderedEnumerable.Last().Name,
                                         orderedEnumerable.Last().SuccessFullBuilds);
                }
            }

            return null; 
        }

        private int SuccessFullBuildInARow(List<Build> builds)
        {
            var firstOrDefault = builds.FirstOrDefault(x => x.Result != "SUCCESS");
            if (firstOrDefault == null) return builds.Count;
            return builds.IndexOf(firstOrDefault);
        }

        private string GetSlowestAndFastedBuild(JenkensProjectsResult jenkensProjectsResult)
        {
            if (jenkensProjectsResult.Jobs != null)
            {
                var orderedEnumerable =
                    jenkensProjectsResult.Jobs.Where(x => x.Builds != null && x.Builds.Count > 3)
                                         .Select(x => new {x.Name, Duration = x.Builds.Average(s => s.Duration)})
                                         .OrderBy(x => x.Duration)
                                         .ToArray();

                if (orderedEnumerable.Length > 2)
                {
                    return string.Format("The fastest build is {0} at {1} per build. The slowest build is {2} at {3} per build",
                                         orderedEnumerable.First().Name,
                                         HumanReadable(TimeSpan.FromMilliseconds(orderedEnumerable.First().Duration),""),
                                         orderedEnumerable.Last().Name,
                                         HumanReadable(TimeSpan.FromMilliseconds(orderedEnumerable.Last().Duration), ""));
                }
            }

            return null;
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
            return timeSpan.TotalDays > 2000 ? "uhhmm who knows" : HumanReadable(timeSpan, " ago");
        }

        public static string HumanReadable(TimeSpan timeSpan, string postfix)
        {
            if (timeSpan.TotalDays > 60)
            {
                return ((int)(timeSpan.TotalDays / 30)) + " months"+postfix;
            }
            if (timeSpan.TotalDays > 30)
            {
                return ((int)(timeSpan.TotalDays / 30)) + " month"+postfix;
            }
            if (timeSpan.TotalDays > 2)
            {
                return ((int)timeSpan.TotalDays) + " days"+postfix;
            }
            if (timeSpan.TotalHours > 23)
            {
                return ((int)timeSpan.TotalDays) + " day"+postfix;
            }
            if (timeSpan.TotalMinutes > 110)
            {
                return ((int)timeSpan.TotalHours) + " hours"+postfix;
            }
            if (timeSpan.TotalMinutes > 60)
            {
                return ((int)timeSpan.TotalHours) + " hour"+postfix;
            }
            if (timeSpan.TotalMinutes > 2)
            {
                return ((int)timeSpan.TotalMinutes) + " minutes"+postfix;
            }
            if (timeSpan.TotalSeconds > 59)
            {
                return ((int)timeSpan.TotalMinutes) + " minute"+postfix;
            }
            if (timeSpan.TotalSeconds > 3)
            {
                return ((int)timeSpan.TotalSeconds) + " seconds"+postfix;
            }
            if (timeSpan.Milliseconds > 1 && string.IsNullOrEmpty(postfix))
            {
                return ((int)timeSpan.Milliseconds) + " milliseconds" + postfix;
            }
            return "just now";
        }

        
    }
}