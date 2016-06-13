using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SetVolumeContextTests : ChatBotTestsBase
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenSetVolumeContext_ShouldShowSetVolumeContextChange()
        {
            // arrange
            Setup();
            _mockIVolumeSetter.Setup(mc => mc.SetVolume(10));
            var messageContext = new MessageContext("set volume 10");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain("volume set to 10");
        }

        [Test]
        public async Task Process_GivenSetVolumeContextWithNumberTooHigh_ShouldReduceTheNumber()
        {
            // arrange
            Setup();
            _mockIVolumeSetter.Setup(mc => mc.SetVolume(10));
            var messageContext = new MessageContext("set volume 100");
            // action
            await _chatBot.Process(messageContext);
            // assert
            messageContext.LastMessages.Should().Contain("volume set to 10");
        }
        
    }
}