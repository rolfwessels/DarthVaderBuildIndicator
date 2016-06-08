using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SayContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenHelpMessage_ShouldProcessMessage()
        {
            // arrange
            Setup();
            _mockITextToSpeech.Setup(mc => mc.Play("Hello loser!")).Returns(Task.FromResult(true));
            var messageContext = new MessageContext("say Hello loser!");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain("Hello loser!");
        }
        
    }
}