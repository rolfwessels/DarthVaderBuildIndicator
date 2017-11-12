using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Chat;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class TellMeAJokeContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenSaySomething_ShouldTellMeAJokeContext()
        {
            // arrange
            Setup();

            var messageContext = new MessageContext("tell me a joke");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().HaveCount(1);
        }
    }
}