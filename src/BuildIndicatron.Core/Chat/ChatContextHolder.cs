using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatContextHolder
    {
        private readonly IFactory _injector;
        private readonly List<IReposonseFlow> _responseFlows = new List<IReposonseFlow>();

        public ChatContextHolder(IFactory injector)
        {
            _injector = injector;
        }

        public List<IReposonseFlow> All {
            get { return _responseFlows; }
        }

        public ChatContextHolder ListenTo<T>() where T : IReposonseFlow
        {
            _responseFlows.Add(_injector.Resolve<T>());
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