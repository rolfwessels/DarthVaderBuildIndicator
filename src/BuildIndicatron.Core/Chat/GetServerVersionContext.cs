using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.SimpleTextSplit;
using Humanizer;
using log4net;

namespace BuildIndicatron.Core.Chat
{
    public class GetServerVersionContext : TextSplitterContextBase<GetServerVersionContext.Server>, IWithHelpText
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHttpLookup _lookup;
        protected readonly Server[] _servers;

        public GetServerVersionContext(IHttpLookup lookup)
        {
            _lookup = lookup;
            _servers = new[]
            {
                new Server() {Name = "prod", Uri = "https://api.22seven.com/heartbeat", ScanCount = 10},
                new Server() {Name = "staging", Uri = "https://test.22seven.com/heartbeat", ScanCount = 1},
            };
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Server> textSplitter)
        {
            textSplitter
                .Map(@"(what|whats)(ANYTHING)(?<name>staging|prod)(ANYTHING)version(ANYTHING)")
                .Map(@"(what|whats)(ANYTHING)version(ANYTHING)(?<name>staging|prod)(ANYTHING)")
                .Map(@"(what|whats)(ANYTHING)version(ANYTHING)");
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context,
            Server server)
        {
            if (string.IsNullOrEmpty(server.Name))
            {
                await
                    context.Respond(string.Format("I will have a look, give me a minute."));
            }
            else
            {
                await
                    context.Respond(string.Format("Checking {0} servers, give me a minute.", server.Name));
            }
            foreach (var serverLink in ForKey(_servers, server.Name))
            {
                await ScanServer(context, serverLink);
            }
            await context.Respond("Those are all the version that I could find.");
        }

        public async Task ScanServer(IMessageContext context, Server link)
        {
            var list = new List<string>();
            for (int i = 0; i < link.ScanCount; i++)
            {
                var serverVersion = await GetVerionForLink(link);

                if (serverVersion == null || list.Contains(serverVersion.ServerName)) continue;
                list.Add(serverVersion.ServerName);
                await
                    context.Respond(string.Format("{0} is on version {1}, released {2} ago.",
                        serverVersion.ServerName, serverVersion.Version,
                        serverVersion.Date.Humanize()));
            }
            if (!list.Any())
            {
                await context.Respond(string.Format("Oops, {0} seems to be down.", link.Uri));
            }
        }

        protected async Task<ServerVersion> GetVerionForLink(Server link)
        {
            try
            {
                var restResponse = await _lookup.Download(link.Uri);
                if (!restResponse.Content.Contains("fine.."))
                {
                    return null;
                }
                var line = Regex.Match(restResponse.Content, @"(.*?),(.*?),(.*?),(.*?)!");
                var serverName = line.Groups[2].Value.Trim();
                var version = line.Groups[3].Value.Trim();
                var date = DateTime.Now - Parse(line);
                return new ServerVersion(serverName, version, date);
            }
            catch (Exception e)
            {
                _log.Error(e.Message, e);
                return null;
            }
        }

        private static DateTime Parse(Match line)
        {
            DateTime dateTime;
            string format = "M/d/yyyy h:mm:ss tt";
            //6/10/2016 2:44:36 PM
            if (DateTime.TryParseExact(line.Groups[4].Value.Trim(), format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            return DateTime.Now;
        }

        #endregion

        #region Implementation of IWithHelpText

        public virtual IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage()
            {
                Call = "what version are we on?",
                Description = "Returns the server versions."
            };
            yield return new HelpMessage()
            {
                Call = "what version is prod on?",
                Description = "Returns production version numbers."
            };
        }

        #endregion

        public IEnumerable<Server> ForKey(IEnumerable<Server> servers, string name)
        {
            return servers.Where(x =>
                    string.IsNullOrEmpty(name) || x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public class Server
        {
            public string Name { get; set; }
            public string Uri { get; set; }
            public bool IsTestServer { get; set; }
            public int ScanCount { get; set; }
        }
    }

    public class ServerVersion
    {
        private readonly string _serverName;
        private readonly string _version;
        private readonly TimeSpan _date;

        public ServerVersion(string serverName, string version, TimeSpan date)
        {
            _serverName = serverName;
            _version = version;
            _date = date;
        }

        public string ServerName
        {
            get { return _serverName; }
        }

        public string Version
        {
            get { return _version; }
        }

        public TimeSpan Date
        {
            get { return _date; }
        }
    }
}