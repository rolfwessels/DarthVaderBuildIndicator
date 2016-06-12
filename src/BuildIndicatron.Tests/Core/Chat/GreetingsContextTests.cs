using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class GreetingsContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenGreetingsContext_ShouldResondWithGreetingsContext()
        {
            // arrange
            Setup();
            
            var messageContext = new MessageContext("hi");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().NotBeEmpty();
        }


   
        
    }
}