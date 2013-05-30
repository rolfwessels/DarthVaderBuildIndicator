using System;
using System.Collections.Generic;
using System.Linq;
using BuildIndicatron.Core.Api.Model;

namespace BuildIndicatron.Core
{
    public class JenkensTextConverter
    {
        public const string FailColor = "red";
        public const string SuccessColor = "blue";
        public const string ResultSuccess = "SUCCESS";

        public string ToSummary(JenkensProjectsResult jenkensProjectsResult)
        {
            string[] strings = ToSummaryList(jenkensProjectsResult).ToArray();
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
                yield return
                    string.Format("{2}, there are currently {0} {1} on jenkins and they are all passing",
                                  jenkensProjectsResult.Jobs.Count,
                                  jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds", GetWelDoneMessage());
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.All(x => x.Color == FailColor))
            {
                yield return
                    string.Format(
                        "There are currently {0} {1} on jenkins and they are all broken. Maybe development is not for you",
                        jenkensProjectsResult.Jobs.Count,
                        jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds");
            }
            else if (jenkensProjectsResult.Jobs != null && jenkensProjectsResult.Jobs.Any())
            {
                List<Job> failedValues = jenkensProjectsResult.Jobs.Where(x => x.Color == FailColor).ToList();


                yield return
                    string.Format("You have failed me, there are currently {0} {2} on jenkins with {1} {3} failing",
                                  jenkensProjectsResult.Jobs.Count,
                                  failedValues.Count,
                                  jenkensProjectsResult.Jobs.Count == 1 ? "build" : "builds",
                                  failedValues.Count == 1 ? "build" : "builds"
                        );
                string[] failedBuildDetail = FailedBuildDetail(failedValues).ToArray();
                foreach (string failedValue in failedBuildDetail)
                {
                    yield return failedValue;
                }
            }
            string slowestAndFastedBuild = GetSlowestAndFastedBuild(jenkensProjectsResult);
            if (slowestAndFastedBuild != null) yield return slowestAndFastedBuild;

            string mostSuccessFullBuild = GetMostSuccessFullBuild(jenkensProjectsResult);
            if (mostSuccessFullBuild != null) yield return mostSuccessFullBuild;
        }

        private string GetWelDoneMessage()
        {
            var messages = new[]
                {
                    "That’s great", "Good job", "Excellent", "I appreciate that", "That’s looking good", "Good work",
                    "Great work", "You’re doing well",
                    "Good to have you on the team", "You made the difference", "Exceptional", "Wonderful",
                    "That is so significant", "Superb",
                    "Perfect", "Just what was needed", "Centre button", "A significant contribution", "Wow", "Fantastic",
                    "Thanks you", "Just what the doctor ordered", "First class", "Nice job", "Way to go", "Far out",
                    "Just the ticket", "You are a legend", "Very professional", "Where would we be without you",
                    "Brilliant", "Top marks",
                    "Impressive", "You hit the target", "Neat", "Cool", "Bullseye", "How did you get so good",
                    "Beautiful", "Just what was wanted",
                    "Right on the money", "Great", "Just right", "Congratulations", "Very skilled",
                    "I’m glad you’re on my team", "It is good to work with you", "You did us proud",
                    "This is going to make us shine", "Well done", "I just love it", "You are fantastic", "Great job",
                    "Professional as usual", "You take the biscuit every time", "I’m proud of you",
                    "Don’t ever leave us","Are you good or what?","The stuff of champions","Cracking job","First class job",
                    "Magnificent","Bravo","Amazing","Simply superb","Triple A","Perfection",
                    "Poetry in motion","Sheer class","World class","Polished performance","Class act",
                    "Unbelievable","Gold plated","Just classic","Super","Now you’re cooking",
                    "You are so good","You deserve a pat on the back","Tremendous job","Unreal",
                    "Treasure","Crash hot","You beauty","The cat’s whiskers","I just can’t thank you enough",
                    "You always amaze me","Magic","Another miracle","Terrific","What a star",
                    "Colossal","Wonderful","Top form","You’re one of a kind","Unique",
                    "Way out","Incredible","Ace"};

            return messages.Random();
        }

        private string GetMostSuccessFullBuild(JenkensProjectsResult jenkensProjectsResult)
        {
            if (jenkensProjectsResult.Jobs != null)
            {
                var orderedEnumerable =
                    jenkensProjectsResult.Jobs.Where(x => x.Builds != null && x.Builds.Count > 3)
                                         .Select(x => new {x.Name, SuccessFullBuilds = SuccessfulBuildInARow(x.Builds)})
                                         .OrderByDescending(x => x.SuccessFullBuilds)
                                         .ToArray();

                if (orderedEnumerable.Length > 2)
                {
                    return
                        string.Format("{0} has {1} succesfull builds in a row. {2} has {3} succesfull builds in a row",
                                      orderedEnumerable.First().Name,
                                      orderedEnumerable.First().SuccessFullBuilds,
                                      orderedEnumerable.Last().Name,
                                      orderedEnumerable.Last().SuccessFullBuilds);
                }
            }

            return null;
        }

        public static int SuccessfulBuildInARow(List<Build> builds)
        {
            Build firstOrDefault = builds.FirstOrDefault(x => x.Result != ResultSuccess);
            if (firstOrDefault == null) return builds.Count;
            return builds.IndexOf(firstOrDefault);
        }

        public static int FailsInARow(List<Build> builds)
        {
            Build firstOrDefault = builds.FirstOrDefault(x => x.Result == ResultSuccess);
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
                    return
                        string.Format(
                            "The fastest build is {0} at {1} per build. The slowest build is {2} at {3} per build",
                            orderedEnumerable.First().Name,
                            HumanReadable(TimeSpan.FromMilliseconds(orderedEnumerable.First().Duration), ""),
                            orderedEnumerable.Last().Name,
                            HumanReadable(TimeSpan.FromMilliseconds(orderedEnumerable.Last().Duration), ""));
                }
            }

            return null;
        }

        private IEnumerable<string> FailedBuildDetail(IEnumerable<Job> failedValues)
        {
            foreach (Job job in failedValues.Where(x => x.LastFailedBuild != null))
            {
                string[] names = job.LastFailedBuild.ChangeSet.items.Select(x => x.author.fullName).Distinct().ToArray();
                if (names.Length == 0) names = new[] {"a ghost"};
                yield return string.Format("The {0} last failed {1}, It was last modified by {2}",
                                           job.Name,
                                           GetLastModifiedDateString(job.LastFailedBuild),
                                           String.Join(" and ", names));
            }
        }

        public string GetLastModifiedDateString(LastFailedBuild lastFailedBuild)
        {
            TimeSpan timeSpan = DateTime.Now - lastFailedBuild.DateTime.AddSeconds(-1);
            return timeSpan.TotalDays > 2000 ? "uhhmm who knows" : HumanReadable(timeSpan, " ago");
        }

        public static string HumanReadable(TimeSpan timeSpan, string postfix)
        {
            if (timeSpan.TotalDays > 60)
            {
                return ((int) (timeSpan.TotalDays/30)) + " months" + postfix;
            }
            if (timeSpan.TotalDays > 30)
            {
                return ((int) (timeSpan.TotalDays/30)) + " month" + postfix;
            }
            if (timeSpan.TotalDays > 2)
            {
                return ((int) timeSpan.TotalDays) + " days" + postfix;
            }
            if (timeSpan.TotalHours > 23)
            {
                return ((int) timeSpan.TotalDays) + " day" + postfix;
            }
            if (timeSpan.TotalMinutes > 110)
            {
                return ((int) timeSpan.TotalHours) + " hours" + postfix;
            }
            if (timeSpan.TotalMinutes > 60)
            {
                return ((int) timeSpan.TotalHours) + " hour" + postfix;
            }
            if (timeSpan.TotalMinutes > 2)
            {
                return ((int) timeSpan.TotalMinutes) + " minutes" + postfix;
            }
            if (timeSpan.TotalSeconds > 59)
            {
                return ((int) timeSpan.TotalMinutes) + " minute" + postfix;
            }
            if (timeSpan.TotalSeconds > 3)
            {
                return ((int) timeSpan.TotalSeconds) + " seconds" + postfix;
            }
            if (timeSpan.Milliseconds > 1 && string.IsNullOrEmpty(postfix))
            {
                return (timeSpan.Milliseconds) + " milliseconds" + postfix;
            }
            return "just now";
        }
    }
}