using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class GetSettingsContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenGetSettingsContext_ShouldResondWithGetSettingsContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get("monitor_channel_jenkins", null)).Returns("#builds");
            var messageContext = new MessageContext("get setting monitor_channel_jenkins");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Last().Should().Be("#builds");
        }

        [Test]
        public async Task Process_GivenSetSettingsWithSpace_ShouldResondWithGetSettingsContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Get())
                .Returns(new Dictionary<string, string>() {{"monitor_channel_jenkins", "#builds"}});
            _mockISettingsManager.Setup(mc => mc.Get("monitor_channel_jenkins", null)).Returns("#builds");
            var messageContext = new MessageContext("get settings");
            // action
            await _chatBot.Process(messageContext);
            messageContext.LastMessages.Should().Contain(x => x == "I have the following: ");
            messageContext.LastMessages.Should().Contain(x => x == "monitor_channel_jenkins");
            await _chatBot.Process(messageContext = new MessageContext("monitor_channel_jenkins"));

            // assert
            messageContext.LastMessages.Last().Should().Be("#builds");
        }
    }
}