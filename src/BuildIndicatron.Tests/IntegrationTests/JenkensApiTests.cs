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
    //[Explicit]
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
          _hostApi = "https://jenkins.my227.net";
//            _hostApi = "http://192.168.1.15:5000/";
        }

        #region Setup/Teardown

        public void Setup()
        {
            _jenkensApi = new JenkensApi(_hostApi, "rolfw", Environment.GetEnvironmentVariable("pssword"));
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
        [Explicit]
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
    }

}