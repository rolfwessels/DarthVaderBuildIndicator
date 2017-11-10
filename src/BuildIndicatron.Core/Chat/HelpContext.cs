using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;

namespace BuildIndicatron.Core.Chat
{
    public class HelpContext : ReposonseFlowBase, IReposonseFlow, IWithHelpText
    {
        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && ContainsText(context, "help"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("I currently have the following functionality:");
            foreach (var helpMessage in chatContextHolder.All.OfType<IWithHelpText>().Dump("d")
                .SelectMany(x => x.GetHelp()))
            {
                stringBuilder.AppendLine(string.Format("{0} - {1}", helpMessage.Call, helpMessage.Description));
            }
            stringBuilder.AppendLine();
            return context.Respond(stringBuilder.ToString());
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "help", Description = "Display the shit you are looking at :-)"};
        }

        #endregion
    }
}