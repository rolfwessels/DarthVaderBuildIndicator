using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class JenkinsMonitorContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenJenkinsMonitorContext_ShouldResondWithJenkinsMonitorContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_channel", It.IsAny<string>()))
                .Returns("jenkins_monitor_channel");
            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_builds", It.IsAny<string>()))
                .Returns("jenkins_monitor_builds");
            var messageContext = new MessageContext("where are you monitoring jenkins?");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("Im currently monitoring jenkins"));
        }


        [Test]
        public async Task Process_GivenCheck_ShouldScanJenkins()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_channel", It.IsAny<string>()))
                .Returns("jenkins_monitor_channel");
            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_builds", It.IsAny<string>()))
                .Returns("jenkins_monitor_builds");
            var messageContext = new MessageContext("check jenkins now?");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("Checking jenkins now."));
        }
    }
}