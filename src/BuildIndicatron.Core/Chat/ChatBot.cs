using System.ComponentModel;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatBot : IChatBot
    {
        private readonly ChatContextHolder _chatContextHolder;

        public ChatBot(IFactory factory)
        {
            _chatContextHolder = new ChatContextHolder(factory)
                .ListenTo<LightsContext>()
                .ListenTo<SayContext>()
                .ListenTo<HelpContext>()
                .ListenTo<RandomJokeResponse>();
        }

        #region Implementation of IChatBot

        public Task Process(IMessageContext context)
        {
            return _chatContextHolder.MessageIn(context);
        }

        #endregion
    }
}