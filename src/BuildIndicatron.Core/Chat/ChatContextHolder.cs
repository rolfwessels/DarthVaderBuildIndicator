using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatContextHolder
    {
        private readonly IFactory _factory;
        private readonly List<IReposonseFlow> _responseFlows = new List<IReposonseFlow>();

        public ChatContextHolder(IFactory factory)
        {
            _factory = factory;
        }

        public ChatContextHolder ListenTo<T>() where T : IReposonseFlow
        {
            
            _responseFlows.Add(_factory.Resolve<T>());
            return this;
        }

        public async Task MessageIn(IMessageContext context)
        {
            foreach (IReposonseFlow reposonseFlow in _responseFlows)
            {
                if (await reposonseFlow.CanRespond(context))
                {
                    await reposonseFlow.Respond(this, context);
                }
            }
        }
    }
}