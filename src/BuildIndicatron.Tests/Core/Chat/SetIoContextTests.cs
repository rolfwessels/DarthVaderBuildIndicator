using System.Threading.Tasks;
using BuildIndicatron.Shared.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SetIoContextTests : ChatBotTestsBase
    {

        [Test]
        public async Task Process_GivenMessage_ShouldRespond()
        {
            // arrange
            Setup();
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightBlue, true));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightGreen, false));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightRed, false));
            // action
            var sampleMessage = new MessageContext("set main light blue");
            await _chatBot.Process(sampleMessage);
            // assert
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("main blue lights are not on")).And.HaveCount(1);
        }

        [Test]
        public async Task Process_GivenMessageWithTwoColors_ShouldRespond()
        {
            // arrange
            Setup();
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightBlue, true));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightGreen, true));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightRed, false));
            // action
            var sampleMessage = new MessageContext("set main light blue green");
            await _chatBot.Process(sampleMessage);
            // assert
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("main green, blue lights are not on")).And.HaveCount(1);
        }
 
        [Test]
        public async Task Process_GivenOtherLight_ShouldRespond()
        {
            // arrange
            Setup();
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.SecondaryLightBlue, true));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.SecondaryLightGreen, true));
            _mockIPinManager.Setup(mc => mc.SetPin(PinName.SecondaryLightRed, false));
            // action
            var sampleMessage = new MessageContext("set secondary light blue green");
            await _chatBot.Process(sampleMessage);
            // assert
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("secondary green, blue lights are not on")).And.HaveCount(1);
        }

    }
}