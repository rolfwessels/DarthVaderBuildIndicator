using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Chat;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Chat
{
    [TestFixture]
    public class SaySomethingTests : ChatBotTestsBase, IWithHelpText
    {
        #region Setup/Teardown

        #endregion
        
        [Test]
        public async Task Process_GivenSaySomething_ShouldSaySomethingContext()
        {
            // arrange
            Setup();
            _mockISoundFilePicker.Setup(mc => mc.PickFile("funny"))
                .Returns("hyp.mp3" );
            _mockIMp3Player.Setup(mc => mc.PlayFile("hyp.mp3"))
                .Returns(Task.FromResult(true));
            var messageContext = new MessageContext("say something funny");
            // action
            await _chatBot.Process(messageContext);
            // assert
        }

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() { Call = "say something _context_", Description = "Allow robot find a _context_ clip and play it." };
        }

        #endregion
    }
}