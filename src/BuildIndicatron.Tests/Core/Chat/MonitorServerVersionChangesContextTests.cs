using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class MonitorServerVersionChangesContextTests : ChatBotTestsBase
    {
        [Test]
        public async Task Process_GivenGetServerVersionContext_ShouldResondWithGetServerVersionContext()
        {
            // arrange
            Setup();
            var restResponse = new RestResponse()
            {
                Content =
                    "<body><header><h1>Everything is fine...</h1></header>Make Coffee, API1, 2.0.1219, 6/10/2016 2:44:36 PM!</body>"
            };
            _mockIHttpLookup.Setup(mc => mc.Download(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(restResponse as IRestResponse));
            var messageContext = new MessageContext("monitor server versions");
            // action
            await _chatBot.Process(messageContext);
            // assert

            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("Scanning servers.")).Should()
                .Contain("Scanning servers.");
            await Task.Delay(500);
            restResponse.Content =
                "<body><header><h1>Everything is fine...</h1></header>Make Coffee, API1, 2.0.1220, 6/10/2016 2:44:36 PM!</body>";
            messageContext.WaitFor(x => x.LastMessages, x => x.Contains("2.0.1220"), 5000).Should()
                .Contain(x => x.Contains("2.0.1220"));
        }
    }
}