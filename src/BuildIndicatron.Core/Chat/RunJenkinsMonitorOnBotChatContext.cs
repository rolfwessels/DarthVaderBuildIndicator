using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;

namespace BuildIndicatron.Core.Chat
{
    public class RunJenkinsMonitorOnBotChatContext : ReposonseFlowBase, IReposonseFlow
    {
        private readonly ISettingsManager _settingsContext;
        private readonly IMonitorJenkins _monitorJenkins;

        public RunJenkinsMonitorOnBotChatContext(ISettingsManager settingsContext , IMonitorJenkins monitorJenkins)
        {
            _settingsContext = settingsContext;
            _monitorJenkins = monitorJenkins;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            var isJenkinsBot = context.FromUser != null && context.FromUser.ToLower().Contains("jenkins");
            return Task.FromResult(isJenkinsBot && context.FromChatHub == _settingsContext.GetBuildChannel());
        }

        public async Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            await _monitorJenkins.Check();
        }

        #endregion
    }
}