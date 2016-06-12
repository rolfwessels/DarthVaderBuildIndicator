using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatBot : IChatBot
    {
        private readonly ChatContextHolder _chatContextHolder;

        public ChatBot(IFactory injector)
        {
            _chatContextHolder = new ChatContextHolder(injector)
                .ListenTo<GreetingsContext>()
                .ListenTo<JenkinsStatusContext>()
                .ListenTo<SetVolumeContext>()
                .ListenTo<GetServerVersionContext>()
                .ListenTo<GetSettingsContext>()
                .ListenTo<SetSettingsContext>()
                .ListenTo<SetIoContext>()
                .ListenTo<SaySomething>()
                .ListenTo<SayContext>()
                .ListenTo<HelpContext>()
                .ListenTo<AboutContext>()
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