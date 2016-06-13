using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class RunJenkinsMonitorOnBotChatContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenRunJenkinsMonitorOnBotChatContext_ShouldResondWithRunJenkinsMonitorOnBotChatContext()
        {
            // arrange
            Setup();

            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_channel", "#builds"))
                .Returns("#builds");
            _mockIMonitorJenkins.Setup(mc => mc.Check()).Returns(Task.FromResult(true));
            var messageContext = new MessageContext("balbal") { FromUser = "jenkins", FromChatHub = "#builds" };
            // action
            await _chatBot.Process(messageContext);
            // assert
        }



        [Test]
        public async Task Process_GivenIncorrectChannel_ShouldResondWithRunJenkinsMonitorOnBotChatContext()
        {
            // arrange
            Setup();

            _mockISettingsManager.Setup(mc => mc.Get("jenkins_monitor_channel", "#builds"))
                .Returns("#builds");
            var messageContext = new MessageContext("balbal") { FromUser = "jenkins" };
            // action
            await _chatBot.Process(messageContext);
            // assert
        }



        
    }
}