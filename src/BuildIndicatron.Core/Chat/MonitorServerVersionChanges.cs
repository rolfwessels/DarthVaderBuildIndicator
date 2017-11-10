using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;
using Humanizer;

namespace BuildIndicatron.Core.Chat
{
    public class MonitorServerVersionChanges : GetServerVersionContext
    {
        private readonly TimeSpan _timeout;

        public MonitorServerVersionChanges(IHttpLookup lookup) : base(lookup)
        {
            _timeout = TimeSpan.FromMinutes(10);
        }

        #region Overrides of GetServerVersionContext

        protected override void Apply(TextSplitter<Server> textSplitter)
        {
            textSplitter
                .Map(@"(alert|monitor|check)(ANYTHING)(?<name>staging|prod)(ANYTHING)version(ANYTHING)")
                .Map(@"(alert|monitor|check)(ANYTHING)version(ANYTHING)(?<name>staging|prod)(ANYTHING)")
                .Map(@"(alert|monitor|check)(ANYTHING)version(ANYTHING)")
                .Map(@"(ANYTHING)version(ANYTHING)changes(ANYTHING) ");
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context,
            Server server)
        {
            if (string.IsNullOrEmpty(server.Name))
            {
                await
                    context.Respond(string.Format("Scanning servers."));
            }
            else
            {
                await
                    context.Respond(string.Format("Scanning {0} servers.", server.Name));
            }

            Task.Run(async () => { await RunOnBackground(context, server); }).FireAndForgetWithLogging();
        }

        private async Task RunOnBackground(IMessageContext context, Server server)
        {
            var doneTime = DateTime.Now + _timeout;
            var list = new List<string>();
            bool report = false;
            while (DateTime.Now < doneTime)
            {
                foreach (var serverLink in ForKey(_servers, server.Name))
                {
                    await Monitor(context, serverLink, list, report);
                    report = true;
                    await Task.Delay(2000);
                }
            }
        }

        private async Task Monitor(IMessageContext context, Server link, List<string> list, bool report)
        {
            for (int i = 0; i < link.ScanCount; i++)
            {
                var serverVersion = await GetVerionForLink(link);
                if (serverVersion == null || list.Contains(serverVersion.ServerName + serverVersion.Version)) continue;
                list.Add(serverVersion.ServerName + serverVersion.Version);
                if (report)
                {
                    await
                        context.Respond(string.Format("{0} now on a new version {1}, released {2} ago.",
                            serverVersion.ServerName, serverVersion.Version,
                            serverVersion.Date.Humanize()));
                }
            }
//            if (!list.Any())
//            {
//                await context.Respond(string.Format("Oops, {0} seems to be down.", link.Uri));
//            }
        }

        #endregion

        public override IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage()
            {
                Call = "monitor server versions?",
                Description = "Alerts you when version numbers change."
            };
            yield return new HelpMessage()
            {
                Call = "monitor staging server versions?",
                Description = "Returns stating version numbers."
            };
        }
    }
}