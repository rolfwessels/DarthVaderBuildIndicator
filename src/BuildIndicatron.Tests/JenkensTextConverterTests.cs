using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api.Model;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestSharp.Deserializers;

namespace BuildIndicatron.Tests
{
    [TestFixture]
    public class JenkensTextConverterTests
    {
        private JenkensTextConverter _jenkensTextConverter;
        private static JenkensProjectsResult _jenkensProjectsResult;

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
            summary.Should().Be("No jenkins data received. Please try again later!");

        }

        [Test]
        public void ToSummary_OnlyOneGoodRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("Yea, there are currently 1 build on jenkins and they are all passing");
        }


        [Test]
        public void ToSummary_OnlyGoodRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "blue", Name = "Build - Zapper IPN Service", HealthReport = new List<Health>() { new Health() { Score = 100 } } } 
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("Yea, there are currently 2 builds on jenkins and they are all passing");
        }

        [Test]
        public void ToSummary_OnlyBadRecords()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "red", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", HealthReport = new List<Health>() { new Health() { Score = 100 } } } 
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should().Be("Oh no, there are currently 2 builds on jenkins and they are all broken. Maybe development is not for you");
        }

        [Test]
        public void ToSummary_OneGoodAndOneBad()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be(
                       "Oh no, there are currently 2 builds on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels");

        }

        [Test]
        public void ToSummary_OneGoodAndTwoBad()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                    }

                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be(
                       "Oh no, there are currently 2 builds on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels");

        }

        [Test]
        public void ToSummary_OneGoodAndOneBadAndOtherDisabledAndGrey()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "Disabled", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "grey", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be(
                       "Oh no, there are currently 4 builds on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels");

        }

        [Test]
        public void ToSummary_OneGoodAndOneBad_DuplicateNames()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                            
                    },
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .StartWith(
                       "Oh no, there are currently 3 builds on jenkins with 2 builds failing");

        }

        [Test]
        public void ToSummary_OneGoodAndOneBad_TwoNames()
        {
            var jenkensProjectsResult = new JenkensProjectsResult()
            {
                Jobs = new List<Job>() { 
                    new Job() { Color = "blue", Name = "Build - Zapper DB", HealthReport = new List<Health>() { new Health() { Score = 100 } } } ,
                    new Job() { Color = "red", Name = "Build - Zapper IPN Service", 
                        LastFailedBuild = new LastFailedBuild() { 
                            ChangeSet = new ChangeSet() { 
                                items = new List<Item>() {
                                    new Item {author = new Author() {fullName = "Rolf Wessels"}},
                                    new Item {author = new Author() {fullName = "Coreen"}},
                                }},
                                Timestamp = DateTime.Now.AddDays(-1).ToEpochTimeStamp()
                        }
                            
                    }
                },
            };
            var summary = _jenkensTextConverter.ToSummary(jenkensProjectsResult);
            summary.Should()
                   .Be("Oh no, there are currently 2 builds on jenkins with 1 build failing. The Build - Zapper IPN Service last failed 1 day ago, It was last modified by Rolf Wessels and Coreen");

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
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddDays(-32).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("1 month ago");
        }

        [Test]
        public void GetLastModifiedDateString_1DayAgo()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddDays(-1.1).ToEpochTimeStamp()});
            lastModifiedDateString.Should().Be("1 day ago");
        }

        [Test]
        public void GetLastModifiedDateString_Days()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddDays(-2.1).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("2 days ago");
        }

        [Test]
        public void GetLastModifiedDateString_HourAgo()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddHours(-1.2).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("1 hour ago");
        }

        [Test]
        public void GetLastModifiedDateString_HoursAgos()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddHours(-3.1).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("3 hours ago");
        }

        [Test]
        public void GetLastModifiedDateString_Minutes()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddMinutes(-30.1).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("30 minutes ago");
        }

        [Test]
        public void GetLastModifiedDateString_Minute()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddMinutes(-1.1).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("1 minute ago");
        }

        [Test]
        public void GetLastModifiedDateString_Now()
        {
            var lastModifiedDateString = _jenkensTextConverter.GetLastModifiedDateString(new LastFailedBuild() { Timestamp = DateTime.Now.AddSeconds(-1).ToEpochTimeStamp() });
            lastModifiedDateString.Should().Be("just now");
        }


        [Test]
        public void UsingTheSampleGetAllValues_build_Count()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummary(jenkensProjectsResult).Should().Contain("currently 43 builds on jenkins");
        }

        [Test]
        public void UsingTheSampleGetAllValues_Fail_Count()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummary(jenkensProjectsResult).Should().Contain("3 builds failing");
        }

        [Test]
        public void UsingTheSampleGetAllValues_LastWorked()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummaryList(jenkensProjectsResult)
                .Should().Contain("The Build - Zapper IPN Service last failed 2 days ago, It was last modified by Sean P Cleworth");

        }

        [Test]
        public void UsingTheSampleGetAllValues_1_day_agoe()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummaryList(jenkensProjectsResult)
                .Should().Contain("The Deploy Dev - ZoomLogin + SampleMerchant last failed 1 day ago, It was last modified by a ghost");

        }

        [Test]
        public void UsingTheSampleGetAllValues_fastest()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummaryList(jenkensProjectsResult)
                .Should().Contain("The fastest build is Deploy Dev - ZapperPayments + ZapperCallback at 10 seconds per build. The slowest build is Build - ZapZap API at 4 minutes per build");

        }

        [Test]
        public void UsingTheSampleGetAllValues_Deployments_InARow()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummaryList(jenkensProjectsResult).Last()
                .Should().Be("Deploy Dev - ZapperPayments + ZapperCallback as 172 succesfull builds in a row. Deploy Dev - ZoomLogin + SampleMerchant has 0 succesfull builds in a row");

        }

        [Test]
        public void UsingTheSampleGetAllValues_LastOne()
        {
            var jenkensProjectsResult = JenkensProjectsResult();
            _jenkensTextConverter.ToSummaryList(jenkensProjectsResult).Last()
                .Should().Be("Deploy Dev - ZapperPayments + ZapperCallback as 172 succesfull builds in a row. Deploy Dev - ZoomLogin + SampleMerchant has 0 succesfull builds in a row");
        }

        
        private static JenkensProjectsResult JenkensProjectsResult()
        {
            if (_jenkensProjectsResult == null)
            {
                var fileStream = File.ReadAllText("save json");
                var jsonDeserializer = new JsonDeserializer();

                _jenkensProjectsResult =
                    jsonDeserializer.Deserialize<JenkensProjectsResult>(new RestResponse() {Content = fileStream});
                _jenkensProjectsResult.Jobs.Count.Should().BeGreaterOrEqualTo(1);
            }

            return _jenkensProjectsResult;
        }
    }

    public static class DateTimeHelper
    {
        public static string ToEpochTimeStamp(this DateTime dsds)
        {
             var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (dsds - epoch).TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}