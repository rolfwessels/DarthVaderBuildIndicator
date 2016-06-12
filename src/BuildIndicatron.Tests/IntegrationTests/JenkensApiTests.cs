using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Helpers;
using FluentAssertions;
using NUnit.Framework;
using log4net;

namespace BuildIndicatron.Tests.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class JenkensApiTests
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static JenkensProjectsResult _allProjects;
        private static string _hostApi;
        private JenkensApi _jenkensApi;

        public JenkensApiTests()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("Log4Net.config"));
        }

        static JenkensApiTests()
        {
            _hostApi = "http://localhost:9999";
//            _hostApi = "http://192.168.1.15:5000/";
        }

        #region Setup/Teardown

        public void Setup()
        {
            _jenkensApi = new JenkensApi(_hostApi);
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public async Task GetAllProjects()
        {
            // arrange
            Setup();
            // action
            var projects = await JenkensProjectsResult();
            // assert
            projects.Should().NotBeNull();
        }


        [Test]
        public async Task GetAllProjects_Translate()
        {
            // arrange
            Setup();
            // action
            var projects = await JenkensProjectsResult();
            var jenkensProjectsResult = new JenkensProjectsResult();
            var jenkensTextConverter = new JenkensTextConverter();
            var summary = jenkensTextConverter.ToSummaryList(projects).ToArray();
            
            foreach (var line in summary)
            {
                _log.Info(line);  
            }
            // assert
            summary.Length.Should().BeGreaterOrEqualTo(1);
            
        }

        [Test]
        public async Task RunAProject_WhenCalled_ShouldExecuteProject()
        {
            // arrange
            Setup();
            var projects = await JenkensProjectsResult();
            var url = projects.Jobs.Select(x => x.Url).First();
            // action
            var parms = new JenkensProjectsBuildRequest("Test", "ttest");
            var buildProject = await _jenkensApi.BuildProject(url);
            // assert
            
        }

        private async Task<JenkensProjectsResult> JenkensProjectsResult()
        {
            
            return _allProjects ?? (_allProjects = await _jenkensApi.GetAllProjects());
        }
        // Oh no, there are currently 43 build on jenkins with 8 build failing. The Build - Zapper IPN Service last failed 24499 months ago, It was last modified by Sean P Cleworth. The Build - ZoomLogin DB + SampleMerchant DB last failed 24499 months ago, It was last modified by Sean Bruins. The Deploy Dev - ZoomLogin + SampleMerchant last failed 24499 months ago, It was last modified by . The XXX LIVE PROD - Build Websites and Test-Deploy last failed 24499 months ago, It was last modified by Mark D. Isaacs and Cobus Bernard and Gino Arpesella and Keiran van Vuuren and Sean P Cleworth
        //{"jobs":[{"name":"Build - Zapper DB","color":"blue","healthReport":[{"score":80}],"lastFailedBuild":{"number":89,"timestamp":1366897906862,"changeSet":{"items":[{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}}]}}},{"name":"Build - Zapper IPN Service","color":"red","healthReport":[{"score":0}],"lastFailedBuild":{"number":178,"timestamp":1366982797000,"changeSet":{"items":[{"author":{"fullName":"Sean P Cleworth"}}]}}},{"name":"Build - ZapperPayments API + ZapperCallback API","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":2,"timestamp":1362725547000,"changeSet":{"items":[]}}},{"name":"Build - ZapZap API","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":120,"timestamp":1366198898000,"changeSet":{"items":[{"author":{"fullName":"Loren G. Rose"}}]}}},{"name":"Build - ZapZap API (Release)","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Build - ZapZap API - Demo","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":113,"timestamp":1366199078000,"changeSet":{"items":[{"author":{"fullName":"Sean Bruins"}},{"author":{"fullName":"Loren G. Rose"}},{"author":{"fullName":"Loren G. Rose"}},{"author":{"fullName":"Loren G. Rose"}}]}}},{"name":"Build - ZapZap API - Dev","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":119,"timestamp":1366198622000,"changeSet":{"items":[{"author":{"fullName":"Sean Bruins"}},{"author":{"fullName":"Loren G. Rose"}},{"author":{"fullName":"Loren G. Rose"}}]}}},{"name":"Build - ZapZap API - Prod (Release)","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Build - ZapZap API - QA","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":6,"timestamp":1363008817000,"changeSet":{"items":[{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Gino Arpesella"}}]}}},{"name":"Build - ZapZap API - Staging (Release)","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Build - ZapZap DB","color":"blue","healthReport":[{"score":80}],"lastFailedBuild":{"number":100,"timestamp":1366897890480,"changeSet":{"items":[{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}}]}}},{"name":"Build - ZapZap DB (Release)","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":6,"timestamp":1363587708000,"changeSet":{"items":[{"author":{"fullName":"Mark D. Isaacs"}}]}}},{"name":"Build - ZoomLogin API + SampleMerchant Website","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":78,"timestamp":1365076899000,"changeSet":{"items":[{"author":{"fullName":"Keiran van Vuuren"}}]}}},{"name":"Build - ZoomLogin DB + SampleMerchant DB","color":"red","healthReport":[{"score":80}],"lastFailedBuild":{"number":110,"timestamp":1366980395000,"changeSet":{"items":[{"author":{"fullName":"Sean Bruins"}}]}}},{"name":"Deploy - Zapper iOS Clients","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":18,"timestamp":1360055739000,"changeSet":{"items":[{"author":{"fullName":"Liam Pillaye"}},{"author":{"fullName":"Liam Pillaye"}},{"author":{"fullName":"Liam Pillaye"}},{"author":{"fullName":"Liam Pillaye"}},{"author":{"fullName":"Liam Pillaye"}}]}}},{"name":"Deploy Demo - ZapperPayments + ZapperCallback","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy Demo - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":3,"timestamp":1363095940000,"changeSet":{"items":[]}}},{"name":"Deploy Demo - ZoomLogin + SampleMerchant","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":16,"timestamp":1365160572000,"changeSet":{"items":[]}}},{"name":"Deploy Dev - ZapperPayments + ZapperCallback","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":950,"timestamp":1366120880000,"changeSet":{"items":[]}}},{"name":"Deploy Dev - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":994,"timestamp":1366623688000,"changeSet":{"items":[]}}},{"name":"Deploy Dev - ZoomLogin + SampleMerchant","color":"red","healthReport":[{"score":40}],"lastFailedBuild":{"number":1310,"timestamp":1366988480574,"changeSet":{"items":[]}}},{"name":"Deploy Mobile Clients - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy Prod - ZapperPayments + ZapperCallback","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy Prod - ZapZap","color":"grey","healthReport":[],"lastFailedBuild":null},{"name":"Deploy Prod - ZoomLogin + SampleMerchant","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy QA - ZapperPayments + ZapperCallback","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":4,"timestamp":1363004222000,"changeSet":{"items":[]}}},{"name":"Deploy QA - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":5,"timestamp":1363074312000,"changeSet":{"items":[]}}},{"name":"Deploy QA - ZoomLogin + SampleMerchant","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":37,"timestamp":1365184463000,"changeSet":{"items":[]}}},{"name":"Deploy Sentinet - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy Staging - ZapperPayments + ZapperCallback","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Deploy Staging - ZapZap","color":"blue","healthReport":[{"score":80}],"lastFailedBuild":{"number":4,"timestamp":1364301286000,"changeSet":{"items":[]}}},{"name":"Deploy Staging - ZoomLogin + SampleMerchant","color":"blue","healthReport":[{"score":60}],"lastFailedBuild":{"number":12,"timestamp":1363588451000,"changeSet":{"items":[]}}},{"name":"DEV - Zapper.Scanner.WP8 - Build Development","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":66,"timestamp":1365575786000,"changeSet":{"items":[{"author":{"fullName":"Rolf Wessels"}}]}}},{"name":"DEV - Zapper.Scanner.WP8 - Build Staging - Release","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":98,"timestamp":1365402388000,"changeSet":{"items":[{"author":{"fullName":"Rolf Wessels"}}]}}},{"name":"Dev - ZapZapMetro - Build Development","color":"blue","healthReport":[{"score":80}],"lastFailedBuild":{"number":32,"timestamp":1366969886000,"changeSet":{"items":[{"author":{"fullName":"coreen"}}]}}},{"name":"DEV - ZapZapMetro - Build Release","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":{"number":103,"timestamp":1362138627000,"changeSet":{"items":[{"author":{"fullName":"Rolf Wessels"}}]}}},{"name":"Old-Build - ZapZap API - Prod","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Old-Build - ZapZap API - Staging (Release)","color":"grey","healthReport":[],"lastFailedBuild":null},{"name":"Old-Build - ZapZap DB (Release)","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Old-Deploy Prod - ZapZap","color":"blue","healthReport":[{"score":100}],"lastFailedBuild":null},{"name":"Old-Deploy Staging - ZapZap","color":"grey","healthReport":[],"lastFailedBuild":null},{"name":"XXX LIVE PROD - Build Websites and Test-Deploy","color":"disabled","healthReport":[{"score":80}],"lastFailedBuild":{"number":18,"timestamp":1349771259000,"changeSet":{"items":[{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Cobus Bernard"}},{"author":{"fullName":"Cobus Bernard"}},{"author":{"fullName":"Cobus Bernard"}},{"author":{"fullName":"Cobus Bernard"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Gino Arpesella"}},{"author":{"fullName":"Sean P Cleworth"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Keiran van Vuuren"}},{"author":{"fullName":"Mark D. Isaacs"}},{"author":{"fullName":"Keiran van Vuuren"}}]}}},{"name":"XXX LIVE PROD -old","color":"disabled","healthReport":[],"lastFailedBuild":null}]}
    }

}