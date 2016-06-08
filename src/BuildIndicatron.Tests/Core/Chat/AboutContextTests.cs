using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class AboutContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenAboutContext_ShouldResondWithAboutContext()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("who are you");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain(x => x.Contains("I am @r2d2, Im currently running on "));
        }
        
    }
}