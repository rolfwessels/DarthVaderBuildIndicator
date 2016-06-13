using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;
using log4net;

namespace BuildIndicatron.Core.Chat
{

    public class JenkinsStatusContext : TextSplitterContextBase<JenkinsStatusContext.Request>, IWithHelpText
    {
        private readonly IJenkinsFactory _apiFactory;
        private readonly ISettingsManager _settingsManager;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public JenkinsStatusContext(IJenkinsFactory apiFactory , ISettingsManager settingsManager)
        {
            _apiFactory = apiFactory;
            _settingsManager = settingsManager;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Request> textSplitter)
        {
            textSplitter
                .Map(@"(ANYTHING)(jenkins) (status)");

        }
        
        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Request server)
        {
            var jenkensApi = _apiFactory.GetBuilder();
            try
            {
                await context.Respond(string.Format("Connecting to {0}.", jenkensApi.Url));
                var allProjects = await jenkensApi.GetAllProjects();
                _log.Info("allProjects:" + allProjects.Dump());
                var jenkensTextConverter = new JenkensTextConverter();
                foreach (var value in jenkensTextConverter.ToSummaryList(allProjects))
                {
                    await context.Respond(value);
                }
                var myBuildingJobs = _settingsManager.GetMyBuildingJobs(allProjects);
                foreach (var value in myBuildingJobs)
                {
                    await context.Respond(string.Format("{0} {1}", value.Name, MapColor(value)));
                }
                
            }
            catch (Exception)
            {
                context.Respond(string.Format("Whoops, there was a problem collecting data from {0}.", jenkensApi.Url)).FireAndForgetWithLogging();
            }
        }

        private string MapColor(Job color)
        {
            if (color.IsProcessing())
            {
                return ":bicyclist:";
            }
            if (color.IsPassed())
            {
                return ":tennis:";
            }
            if (color.IsFailed())
            {
                return ":red_circle:";
            }

            return ":volleyball:";
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "jenkins status", Description = "Set jenkins status."};
        }

        #endregion

        public class Request
        {
        }
    }

    
}