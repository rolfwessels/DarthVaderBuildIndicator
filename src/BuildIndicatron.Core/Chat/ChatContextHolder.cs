using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatContextHolder
    {
        private readonly IInjector _injector;
        private readonly List<IReposonseFlow> _responseFlows = new List<IReposonseFlow>();

        public ChatContextHolder(IInjector injector)
        {
            _injector = injector;
        }

        public ChatContextHolder ListenTo<T>() 
        {
            _responseFlows.Add( (IReposonseFlow) _injector.Resolve<T>());
            return this;
        }

        public async Task MessageIn(IMessageContext context)
        {
            foreach (var reposonseFlow in _responseFlows)
            {
                if (await reposonseFlow.CanRespond(context))
                {
                    await reposonseFlow.Respond(this,context);
                    break;
                }
            }
            
        }
    }
}