using System.Threading.Tasks;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Tests.Core.Chat;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Chat
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
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("lights set")).And.HaveCount(1);
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
            sampleMessage.LastMessages.Should().Contain(x => x.Contains("lights set")).And.HaveCount(1);
        }

    }
}