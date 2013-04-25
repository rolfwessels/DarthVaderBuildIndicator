using System;
using System.Collections.Generic;
using System.Linq;
using BuildIndicatron.Core.Api.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests
{
    [TestFixture]
    public class ConvertJenkensOutputToString
    {
        private JenkensTextConverter _jenkensTextConverter;

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _jenkensTextConverter = new JenkensTextConverter();
        }

        [TearDown]
        public void TearDown()
        {
            
        }

        #endregion

        [Test]
        public void ToSummary_NoRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult();
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("No jenkins data recieved. Please try again later!");

        }

        [Test]
        public void ToSummary_OnlyGoodRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                jobs = new List<Job>() { 
                    new Job() { color = "blue", name = "Build - Zapper DB", healthReport = new List<Health>() { new Health() { score = 100 } } } ,
                    new Job() { color = "blue", name = "Build - Zapper IPN Service", healthReport = new List<Health>() { new Health() { score = 100 } } } 
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("Yea, there are currently 2 build on jenkins and they are all passing");
        }

        [Test]
        public void ToSummary_OnlyBadRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                jobs = new List<Job>() { 
                    new Job() { color = "red", name = "Build - Zapper DB", healthReport = new List<Health>() { new Health() { score = 100 } } } ,
                    new Job() { color = "red", name = "Build - Zapper IPN Service", healthReport = new List<Health>() { new Health() { score = 100 } } } 
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("Oh no, there are currently 2 build on jenkins and they are all broken. Maybe development is not for you");
        }

        [Test]
        public void ToSummary_OneGoodAndOneBad()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                jobs = new List<Job>() { 
                    new Job() { color = "blue", name = "Build - Zapper DB", healthReport = new List<Health>() { new Health() { score = 100 } } } ,
                    new Job() { color = "red", name = "Build - Zapper IPN Service", 
                        lastFailedBuild = new LastFailedBuild() { 
                            changeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                timestamp = DateTime.Now.AddDays(-1).Ticks.ToString()
                        }
                            
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be(
                       "Oh no, there are currently 2 build on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels");

        }

        [Test]
        public void ToSummary_OneGoodAndOneBad_DuplicateNames()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                jobs = new List<Job>() { 
                    new Job() { color = "blue", name = "Build - Zapper DB", healthReport = new List<Health>() { new Health() { score = 100 } } } ,
                    new Job() { color = "red", name = "Build - Zapper IPN Service", 
                        lastFailedBuild = new LastFailedBuild() { 
                            changeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                timestamp = DateTime.Now.AddDays(-1).Ticks.ToString()
                        }
                            
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be(
                       "Oh no, there are currently 2 build on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels");

        }

        [Test]
        public void ToSummary_OneGoodAndOneBad_TwoNames()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                jobs = new List<Job>() { 
                    new Job() { color = "blue", name = "Build - Zapper DB", healthReport = new List<Health>() { new Health() { score = 100 } } } ,
                    new Job() { color = "red", name = "Build - Zapper IPN Service", 
                        lastFailedBuild = new LastFailedBuild() { 
                            changeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                    new Item {author = new Author() {fullName = "Coreen"}},
                                }},
                                timestamp = DateTime.Now.AddDays(-1).Ticks.ToString()
                        }
                            
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be("Oh no, there are currently 2 build on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels and Coreen");

        }

        [Test]
        public void GetLastModifiedDateString_No_ValueSet()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild());
            lastModifiedDateString.Should().Be("uhhmm who knows");
        }

        [Test]
        public void GetLastModifiedDateString_Month()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddDays(-32).Ticks.ToString() });
            lastModifiedDateString.Should().Be("1 month ago");
        }

        [Test]
        public void GetLastModifiedDateString_1DayAgo()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddDays(-1).Ticks.ToString()});
            lastModifiedDateString.Should().Be("1 day ago");
        }

        [Test]
        public void GetLastModifiedDateString_Days()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddDays(-2.1).Ticks.ToString() });
            lastModifiedDateString.Should().Be("2 days ago");
        }

        [Test]
        public void GetLastModifiedDateString_HourAgo()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddHours(-1.2).Ticks.ToString() });
            lastModifiedDateString.Should().Be("1 hour ago");
        }

        [Test]
        public void GetLastModifiedDateString_HoursAgos()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddHours(-3.1).Ticks.ToString() });
            lastModifiedDateString.Should().Be("3 hours ago");
        }

        [Test]
        public void GetLastModifiedDateString_Minutes()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddMinutes(-30).Ticks.ToString() });
            lastModifiedDateString.Should().Be("30 minutes ago");
        }

        [Test]
        public void GetLastModifiedDateString_Minute()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddMinutes(-1).Ticks.ToString() });
            lastModifiedDateString.Should().Be("1 minute ago");
        }

        [Test]
        public void GetLastModifiedDateString_Now()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { timestamp = DateTime.Now.AddSeconds(-1).Ticks.ToString() });
            lastModifiedDateString.Should().Be("just now");
        }
    }

    public class JenkensTextConverter
    {
        public string ToSummary(JenkensProjectsResult jenkensProjectsResult)
        {
            if (jenkensProjectsResult.jobs != null && jenkensProjectsResult.jobs.All(x => x.color == "blue"))
            {
                return string.Format("Yea, there are currently {0} build on jenkins and they are all passing",
                                     jenkensProjectsResult.jobs.Count);
            }
            if (jenkensProjectsResult.jobs != null && jenkensProjectsResult.jobs.All(x => x.color != "blue"))
            {
                return string.Format("Oh no, there are currently {0} build on jenkins and they are all broken. Maybe development is not for you",
                                     jenkensProjectsResult.jobs.Count);
            }
            if (jenkensProjectsResult.jobs != null && jenkensProjectsResult.jobs.Any())
            {
                var failedValues = jenkensProjectsResult.jobs.Where(x => x.color != "blue").ToList();
                var failedBuildDetail = FailedBuildDetail(failedValues).ToArray();

                return string.Format("Oh no, there are currently {0} build on jenkins with {1} build failing. {2}",
                                     jenkensProjectsResult.jobs.Count,
                                     failedValues.Count,
                                     string.Join(". ", failedBuildDetail)) ;
            }
            return "No jenkins data recieved. Please try again later!";
        }

        private IEnumerable<string> FailedBuildDetail(IEnumerable<Job> failedValues)
        {
            foreach (var job in failedValues.Where(x=>x.lastFailedBuild != null ))
            {
                var names = job.lastFailedBuild.changeSet.items.Select(x => x.author.fullName).Distinct().ToArray();
                yield return string.Format("The {0} last failed {1}, It was last modified by {2}", 
                    job.name , 
                    GetLastModifiedDateString(job.lastFailedBuild),
                    String.Join(" and ",names));
            }
        }

        public string GetLastModifiedDateString(LastFailedBuild lastFailedBuild)
        {
            long ticks;
            if (long.TryParse(lastFailedBuild.timestamp, out ticks))
            {
                var timeSpan = DateTime.Now - new DateTime(ticks);
                return HumanReadable(timeSpan);
            }
            return "uhhmm who knows";
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
            if (timeSpan.TotalMinutes > 1)
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