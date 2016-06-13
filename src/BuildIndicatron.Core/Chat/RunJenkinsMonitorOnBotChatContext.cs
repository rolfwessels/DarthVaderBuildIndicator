using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;

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
            var buildChannel = _settingsContext.GetBuildChannel();
            context.Respond("User:" + context.FromUser);
            return Task.FromResult(context.FromChatHub == buildChannel && context.FromUser.ToLower().Contains("jenkins"));
        }

        public async Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            await _monitorJenkins.Check();
        }

        #endregion
    }
}