using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SetSettingsContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenSetSettingsContext_ShouldResondWithSetSettingsContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Set("monitor_channel_jenkins", "#builds"));
            var messageContext = new MessageContext("set setting monitor_channel_jenkins #builds");
            // action
            await _chatBot.Process(messageContext);
            // assert
        }


        [Test]
        public async Task Process_GivenSetSettingsWithSpace_ShouldResondWithSetSettingsContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Set("monitor_channel_jenkins", "builds asdf ss"));
            var messageContext = new MessageContext("set setting monitor_channel_jenkins");
            // action
            await _chatBot.Process(messageContext);
            messageContext.LastMessages.Last().Should().Be("what is the value?");
            await _chatBot.Process(new MessageContext("builds asdf ss"));
            // assert
        }

        [Test]
        public async Task Process_GivenSetSettingsWithNoKey_ShouldAskForKey()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Set("monitor_channel_jenkins", "builds asdf ss"));
            var messageContext = new MessageContext("set setting");
            // action
            await _chatBot.Process(messageContext);
            messageContext.LastMessages.Last().Should().Be("what is the key?");
            await _chatBot.Process(messageContext = new MessageContext("monitor_channel_jenkins"));
            messageContext.LastMessages.Last().Should().Be("what is the value?");
            await _chatBot.Process(messageContext = new MessageContext("builds asdf ss"));
            // assert
        }
        
        [Test]
        public async Task Process_GivenInValidMatch_ShouldAskForKey()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("set setting");
            // action
            await _chatBot.Process(messageContext);
            messageContext.LastMessages.Last().Should().Be("what is the key?");
            await _chatBot.Process(messageContext = new MessageContext("cancel"));
            messageContext.LastMessages.Last().Should().Be("nevermind");
            await _chatBot.Process(messageContext = new MessageContext("help"));
            messageContext.LastMessages.Last().Should().Contain("functionality");
            // assert
        }
        
    }
}