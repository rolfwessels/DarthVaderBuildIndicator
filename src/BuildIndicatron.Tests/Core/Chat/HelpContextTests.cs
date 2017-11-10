using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class HelpContextTests : ChatBotTestsBase
    {
        [Test]
        public async Task Process_GivenMessage_ShouldRespond()
        {
            // arrange
            Setup();
            // action
            var sampleMessage = new MessageContext("help me out");
            await _chatBot.Process(sampleMessage);
            // assert
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("help"));
        }
    }
}