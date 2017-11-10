using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api.Model;
using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class JenkinsStatusContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion

        [Test]
        public async Task Process_GivenJenkinsStatusContext_ShouldResondWithJenkinsStatusContext()
        {
            // arrange
            Setup();
            _mockIJenkensApi.Setup(mc => mc.Url)
                .Returns("Test");
            var messageContext = new MessageContext("jenkins status");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should()
                .Contain(x => x.StartsWith("Whoops, there was a problem collecting data from"));
        }

        [Test]
        public async Task Process_Given_ShouldResondWithJenkinsStatusContext()
        {
            // arrange
            Setup();
            _mockIJenkensApi.Setup(mc => mc.Url)
                .Returns("Test");
            var jobs = Builder<Job>.CreateListOfSize(2).Build();
            _mockIJenkensApi.Setup(mc => mc.GetAllProjects())
                .Returns(Task.FromResult(new JenkensProjectsResult() {Jobs = jobs.ToList()}));
            var messageContext = new MessageContext("jenkins status");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should()
                .Contain(x => x.Contains("there are currently 2 builds on jenkins"));
        }
    }
}