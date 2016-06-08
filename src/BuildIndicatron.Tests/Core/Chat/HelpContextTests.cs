using System.Threading.Tasks;
using BuildIndicatron.Core.Chat;
using BuildIndicatron.Server.Setup;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class HelpContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenHelpMessage_ShouldProcessMessage()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("help me out");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain("helping you now");
        }
        
    }
}