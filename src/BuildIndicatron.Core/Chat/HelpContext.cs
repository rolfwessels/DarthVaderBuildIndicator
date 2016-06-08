using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public class HelpContext : ReposonseFlowBase, IReposonseFlow
    {
        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) || ContainsText(context,"help"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            return context.Respond("helping you now");
        }

        #endregion
    }
}