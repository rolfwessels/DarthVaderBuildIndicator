using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.Settings;

namespace BuildIndicatron.Core.Chat
{
    public class SetSettingsContext : ReposonseFlowBase, IReposonseFlow, IWithHelpText
    {
        private readonly ISettingsManager _settingsContext;


        public SetSettingsContext(ISettingsManager settingsContext)
        {
            _settingsContext = settingsContext;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "set setting"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var extractStartsWith = ExtractStartsWith(context, "say");
            
            return context.Respond(extractStartsWith);
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "set setting [key] [value]",Description = "Settings some settings."};
        }

        #endregion
    }
}