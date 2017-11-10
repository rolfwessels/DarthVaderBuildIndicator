using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class GetServerVersionContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenGetServerVersionContext_ShouldResondWithGetServerVersionContext()
        {
            // arrange
            Setup();
            _mockIHttpLookup.Setup(mc => mc.Download(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new RestResponse()
                {
                    Content =
                        "<body><header><h1>Everything is fine...</h1></header>Make Coffee, API1, 2.0.1220, 6/10/2016 2:44:36 PM!</body>"
                } as IRestResponse));
            var messageContext = new MessageContext("what version are we on");
            // action
            await _chatBot.Process(messageContext);
            // assert

            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("I will have a look, give me a minute."))
                .Should().Contain("I will have a look, give me a minute.");
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("API1")).Should()
                .Contain(x => x.Contains("API1"));
        }

        [Test]
        public async Task Process_GivenGetServerVersionContext_ShouldOnlyCheckThat()
        {
            // arrange
            Setup();
            _mockIHttpLookup.Setup(mc => mc.Download(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new RestResponse()
                {
                    Content =
                        "<body><header><h1>Everything is fine...</h1></header>Make Coffee, API1, 2.0.1220, 6/10/2016 2:44:36 PM!</body>"
                } as IRestResponse));
            var messageContext = new MessageContext("what prod version are we on");
            // action
            await _chatBot.Process(messageContext);
            // assert

            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("Checking prod servers, give me a minute."))
                .Should().Contain("Checking prod servers, give me a minute.");
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("API1")).Should()
                .Contain(x => x.Contains("1 week ago"));
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("API1")).Should()
                .Contain(x => x.Contains("API1"));
        }


        [Test]
        public async Task Process_GivenInvalidContext_ShouldResondWithGetServerVersionContext()
        {
            // arrange
            Setup();
            _mockIHttpLookup.Setup(mc => mc.Download(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new RestResponse() { } as IRestResponse));
            var messageContext = new MessageContext("what version are we on");
            // action
            await _chatBot.Process(messageContext);
            // assert

            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("I will have a look, give me a minute."))
                .Should().Contain("I will have a look, give me a minute.");
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("Oops")).Should()
                .Contain(x => x.Contains("Oops"));
        }
    }
}