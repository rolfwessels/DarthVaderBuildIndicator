using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SetSettingsContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenSetSettingsContext_ShouldResondWithSetSettingsContext()
        {
            // arrange
            Setup();
            _mockISettingsManager.Setup(mc => mc.Set("monitor_channel_jenkins", " #builds"));
            var messageContext = new MessageContext("set setting monitor_channel_jenkins #builds");
            // action
            await _chatBot.Process(messageContext);
            // assert
        }
        
    }
}