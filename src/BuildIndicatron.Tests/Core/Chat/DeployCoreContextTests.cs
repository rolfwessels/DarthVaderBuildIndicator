using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Helpers;
using FizzWare.NBuilder;
using FluentAssertions;
using log4net;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class DeployCoreContextTests : ChatBotTestsBase
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

       

        [Test]
        public async Task Process_GivenDeployCoreContext_ShouldResondWithDeployCoreContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("deployer_staging_builds",It.IsAny<string>()))
                .Returns("SampleUat");
            _mockISettingsManager.Setup(mc => mc.Get("deployer_prod_builds", It.IsAny<string>()))
                .Returns("SampleProd1,SampleProd1");
            var jobs  = Builder<Job>.CreateListOfSize(3).Build();
            _mockIJenkensApi.Setup(mc => mc.GetAllProjects())
                .Returns(Task.FromResult(new JenkensProjectsResult() {Jobs = jobs.ToList()}));   
            var messageContext = new MessageContext("deploy");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("Please specify a project name for Staging deploy, 'SampleUat'"));
        }

        [Test]
        public async Task Process_GivenCorrectUatBuildButWrongProd_ShouldLetTheUserKnow()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("deployer_staging_builds",It.IsAny<string>()))
                .Returns("Name1");
            _mockISettingsManager.Setup(mc => mc.Get("deployer_prod_builds", It.IsAny<string>()))
                .Returns("SampleProd1,SampleProd1");
            var jobs  = Builder<Job>.CreateListOfSize(3).Build();
            _mockIJenkensApi.Setup(mc => mc.GetAllProjects())
                .Returns(Task.FromResult(new JenkensProjectsResult() {Jobs = jobs.ToList()}));   
            var messageContext = new MessageContext("deploy");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("Please specify a project name for Prod deploy, 'SampleProd1' could not be found"));
        }

        [Test]
        public void Process_GivenCorrectBuild_ShouldBuild()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("build_processing_timeout_minutes", It.IsAny<int>()))
                .Returns(1);
            _mockISettingsManager.Setup(mc => mc.Get("deployer_staging_builds",It.IsAny<string>()))
                .Returns("Name1");
            _mockISettingsManager.Setup(mc => mc.Get("deployer_prod_builds", It.IsAny<string>()))
                .Returns("Name2,Name3");
            _mockIJenkensApi.Setup(mc => mc.BuildProject("Url1"))
                .Returns(Task.FromResult(new JenkensProjectsResult()));
            var jobs  = Builder<Job>.CreateListOfSize(3).Build();
            _mockIJenkensApi.Setup(mc => mc.GetAllProjects())
                .Returns(Task.FromResult(new JenkensProjectsResult() {Jobs = jobs.ToList()}));   
            var messageContext = new MessageContext("deploy");
            // action
            _chatBot.Process(messageContext);
            // assert
            WaitFor(messageContext.LastMessages, "Starting the staging builds.");
            WaitFor(messageContext.LastMessages, "*Name1* - status Color1");
            WaitFor(messageContext.LastMessages, "Waiting for the job to start.");
            jobs.First().Color = "blue_anime";
            WaitFor(messageContext.LastMessages, "Waiting for the job to finish.");
            jobs.First().Color = "blue";
        }

        [Test]
        [Explicit]
        public void Process_GivenProdBuild_ShouldBuild()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("build_processing_timeout_minutes", It.IsAny<int>()))
                .Returns(1);
            _mockISettingsManager.Setup(mc => mc.Get("deployer_staging_builds",It.IsAny<string>()))
                .Returns("Name1");
            _mockISettingsManager.Setup(mc => mc.Get("deployer_prod_builds", It.IsAny<string>()))
                .Returns("Name2,Name3");
            _mockIJenkensApi.Setup(mc => mc.BuildProject("Url2"))
                .Returns(Task.FromResult(new JenkensProjectsResult()));
            _mockIJenkensApi.Setup(mc => mc.BuildProject("Url3"))
                .Returns(Task.FromResult(new JenkensProjectsResult()));
            var jobs  = Builder<Job>.CreateListOfSize(3).Build();
            _mockIJenkensApi.Setup(mc => mc.GetAllProjects())
                .Returns(Task.FromResult(new JenkensProjectsResult() {Jobs = jobs.ToList()}));   
            var messageContext = new MessageContext("deploy prod");
            // action
            _chatBot.Process(messageContext);
            // assert
            WaitFor(messageContext.LastMessages, "Starting the Prod builds.");
            WaitFor(messageContext.LastMessages, "Waiting for the job to start.");
            jobs.Skip(1).First().Color = "blue_anime";
            WaitFor(messageContext.LastMessages, "Waiting for the job to finish.");
            messageContext.LastMessages.Clear();
            jobs.Skip(1).First().Color = "blue";
            
            WaitFor(messageContext.LastMessages, "Waiting for the job to start.");
            jobs.Skip(2).First().Color = "blue_anime";
            WaitFor(messageContext.LastMessages, "Waiting for the job to finish.");
            jobs.Skip(2).First().Color = "blue";

            WaitFor(messageContext.LastMessages, "Done.");

        }

        private static void WaitFor(IEnumerable<string> lastMessages, string waitingForTheJobToStart)
        {
            lastMessages.WaitFor(x => x, x => x.Contains(waitingForTheJobToStart),10000);
            lastMessages.Should().Contain(x => x.Contains(waitingForTheJobToStart));
        }
    }
}