using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class AboutContextTests : ChatBotTestsBase
    {
        [Test]
        public async Task Process_GivenAboutContext_ShouldResondWithAboutContext()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("who are you");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("working from home today"));
        }

        [Test]
        public async Task Process_GivenWhereAreYou_ShouldResondWithAboutContext()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("where are you?");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("working from home today"));
        }
    }
}