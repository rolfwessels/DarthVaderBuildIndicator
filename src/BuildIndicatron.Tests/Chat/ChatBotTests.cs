using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Chat
{
    [TestFixture]
    public class ChatBotTests : ChatBotTestsBase
    {

      
        [Test]
        public async Task Process_GivenMessage_ShouldRespond()
        {
            // arrange
            Setup();
            // action
            var sampleMessage = new SampleMessage("hjhljkh", true);
            await _chatBot.Process(sampleMessage);
            // assert
            sampleMessage.Responses.Should().Contain(x => x.Contains("help"));
        }

    }
}