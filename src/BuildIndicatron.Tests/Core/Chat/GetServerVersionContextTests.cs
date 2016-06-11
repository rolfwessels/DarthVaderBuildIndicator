using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class GetServerVersionContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        [Explicit]
        public async Task Process_GivenGetServerVersionContext_ShouldResondWithGetServerVersionContext()
        {
            // arrange
            Setup();
            var messageContext = new MessageContext("what version are we on");
            // action
            await _chatBot.Process(messageContext);
            // assert
            
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("I will have a look, give me a minute.")).Should().Contain("I will have a look, give me a minute.");
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("API2")).Should().Contain(x => x.Contains("API2"));
        }


    }
}