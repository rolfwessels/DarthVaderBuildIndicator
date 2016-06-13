using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Chat;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class TellMeAInsultContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenSaySomething_ShouldTellMeAInsultContext()
        {
            // arrange
            Setup();

            var messageContext = new MessageContext("tell me a insult");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().HaveCount(1);
        }

      
    }
}