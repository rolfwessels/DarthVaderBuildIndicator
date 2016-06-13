using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
    public class JenkinsMonitorContext : TextSplitterContextBase<JenkinsMonitorContext.Meta> , IWithHelpText
    {
        private readonly ISettingsManager _settingsContext;
        private readonly IMonitorJenkins _monitorJenkins;

        public JenkinsMonitorContext(ISettingsManager settingsContext , IMonitorJenkins monitorJenkins)
        {
            _settingsContext = settingsContext;
            _monitorJenkins = monitorJenkins;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter
                .Map(@"(?<where>where)(ANYTHING)(monitor)(ANYTHING)(jenkins)(ANYTHING)")
                .Map(@"(?<check>check)(ANYTHING)(jenkins)(ANYTHING)"); 
        }
        
        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Meta server)
        {
            var buildChannel = _settingsContext.GetBuildChannel();
            var buildChannels = _settingsContext.GetMyBuildingJobs();
            if (!string.IsNullOrEmpty(server.Where))
                await context.Respond(string.Format("Im currently monitoring jenkins on {0} for projects {1}", buildChannel, buildChannels.StringJoin()));
            if (!string.IsNullOrEmpty(server.Check))
            {
                await context.Respond(string.Format("Checking jenkins now."));
                await _monitorJenkins.Check();
                await context.Respond(string.Format("lights set."));
            }
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() { Call = "where are you monitoring jenkins", Description = "See where jenkins is being monitored." };
            yield return new HelpMessage() { Call = "check jenkins now", Description = "Check jenkins status now." };
        }

        #endregion

        public class Meta
        {
            public string Where { get; set; }
            public string Check { get; set; }
        }
    }
}