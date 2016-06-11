using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;
using log4net;

namespace BuildIndicatron.Core.Chat
{

    public class JenkinsStatusContext : TextSplitterContextBase<JenkinsStatusContext.Request>, IWithHelpText
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFactory _factory;

        public JenkinsStatusContext(IFactory factory)
        {
            _factory = factory;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Request> textSplitter)
        {
            textSplitter.Map(@"(ANYTHING)(jenkins) (status)");
        }

       

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Request server)
        {
            var jenkensApi = _factory.Resolve<IJenkensApi>();
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
                
            }
            catch (Exception)
            {
                context.Respond(string.Format("Whoops, there was a problem collecting data from {0}.", jenkensApi.Url));
            }
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