using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class TellMeAQuotesContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenSaySomething_ShouldTellMeAQuotesContext()
        {
            // arrange
            Setup();

            var messageContext = new MessageContext("tell me a quote");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().HaveCount(1);
        }
    }
}