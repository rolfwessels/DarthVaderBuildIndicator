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
                .ListenTo<DeployCoreContext>()
                .ListenTo<JenkinsMonitorContext>()
                .ListenTo<GreetingsContext>()
                .ListenTo<JenkinsStatusContext>()
                .ListenTo<SetVolumeContext>()
                .ListenTo<GetServerVersionContext>()
                .ListenTo<MonitorServerVersionChanges>()
                .ListenTo<GetSettingsContext>()
                .ListenTo<SetSettingsContext>()
                .ListenTo<SetIoContext>()
                .ListenTo<SaySomethingContext>()
                .ListenTo<SayContext>()
                .ListenTo<HelpContext>()
                .ListenTo<AboutContext>()
                .ListenTo<RunJenkinsMonitorOnBotChatContext>()
                .ListenTo<TellMeAJokeContext>()
                .ListenTo<TellMeAInsultContext>()
                .ListenTo<TellMeAQuotesContext>()
                .ListenTo<FailedToRespondContext>()
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