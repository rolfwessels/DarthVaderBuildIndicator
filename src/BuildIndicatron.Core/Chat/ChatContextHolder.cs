using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class ChatContextHolder
    {
        private readonly IFactory _injector;
        private readonly List<IReposonseFlow> _responseFlows = new List<IReposonseFlow>();
        private readonly List<IReposonseFlow> _oneTimeFlow = new List<IReposonseFlow>();

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
            var reposonseFlows = _oneTimeFlow.ToArray();
            _oneTimeFlow.Clear();
            foreach (var reposonseFlow in reposonseFlows)
            {
                if (await reposonseFlow.CanRespond(context))
                {
                    await reposonseFlow.Respond(this, context);
                    break;
                }
            }
            
            foreach (var reposonseFlow in _responseFlows)
            {
                if (await reposonseFlow.CanRespond(context))
                {
                    await reposonseFlow.Respond(this,context);
                    break;
                }
            }
            
        }

        public void AddOneTime(IReposonseFlow easyContext)
        {
            if (!_oneTimeFlow.Any())
            {
                _oneTimeFlow.Add(new QuickQuickTextMatch(x=>x.Map("cancel").Map("nevermind").Map("exit"),(holder, context) => context.Respond("nevermind") )); 
            }
            _oneTimeFlow.Add(easyContext);
        }
    }
}