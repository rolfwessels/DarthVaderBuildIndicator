using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SayContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenSayContext_ShouldResondWithSayContext()
        {
            // arrange
            Setup();
            _mockITextToSpeech.Setup(mc => mc.Play("Hello loser!", It.IsAny<IVoiceEnhancer>()))
                .Returns(Task.FromResult(true));
            var messageContext = new MessageContext("say Hello loser!");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain("Hello loser!");
        }
    }
}