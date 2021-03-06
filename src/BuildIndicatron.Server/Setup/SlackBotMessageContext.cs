using System.Threading.Tasks;
using BuildIndicatron.Core.Chat;
using SlackConnector.Models;

namespace BuildIndicatron.Server.Setup
{
    public class SlackBotMessageContext : IMessageContext
    {
        private readonly SlackBotServer _slackBotServer;
        private readonly SlackMessage _message;

        public SlackBotMessageContext(SlackBotServer slackBotServer, SlackMessage message)
        {
            _slackBotServer = slackBotServer;
            _message = message;
        }

        public string Text { get; set; }

        public bool IsDirectedAtMe {
            get { return _message.MentionsBot || _message.ChatHub.Type == SlackChatHubType.DM; }
        }

        public bool IsBotMessage {
            get { return true; }
        }

        public string FromChatHub
        {
            get { return _message.ChatHub.Name; }
        }

        public string FromUser { get { return _message.User.Name??""; } }

        public Task Respond(string message)
        {

            return _slackBotServer.SayTo(_message.ChatHub, message);
        }
    }
}