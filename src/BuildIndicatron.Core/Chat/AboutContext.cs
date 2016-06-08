using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{


    public class AboutContext : ReposonseFlowBase, IReposonseFlow, IWithHelpText
    {
        

        
        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "who are you"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {

            return context.Respond(string.Format("I am @r2d2, Im currently running on {0}", IpAddressHelper.GetLocalIpAddresses().StringJoin(" or ")));
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() { Call = "who are you", Description = "More about the bot and location." };
        }

        #endregion
    }
}