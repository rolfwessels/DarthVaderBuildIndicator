using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatBot : IChatBot
    {
        private readonly ChatContextHolder _chatContextHolder;

        public ChatBot(IInjector injector)
        {
            _chatContextHolder = new ChatContextHolder(injector)
                .ListenTo<SetIoContext>()
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