using System.IO;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.IntegrationTests
{
    [TestFixture]
    public class JenkensApiTests
    {
        private static JenkensProjectsResult allProjects;

        public JenkensApiTests()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("Log4Net.config"));
        }

        [Test]
        public async Task GetAllProjects()
        {
            var allProjects = await JenkensProjectsResult();
            allProjects.Should().NotBeNull();
        }

        private static async Task<JenkensProjectsResult> JenkensProjectsResult()
        {
            var jenkensApi = new JenkensApi();
            if (allProjects == null)
            {
                allProjects = await jenkensApi.GetAllProjects();
            }
            return allProjects;
        }
    }

}