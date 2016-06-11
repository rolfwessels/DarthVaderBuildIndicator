using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;
using Humanizer;
using RestSharp;

namespace BuildIndicatron.Core.Chat
{

    public class GetServerVersionContext : TextSplitterContextBase<GetServerVersionContext.Server>, IWithHelpText
    {
        private readonly Server[] _servers;

        public GetServerVersionContext()
        {
            _servers = new[] { 
		        new Server() { Name = "API", Uri = "https://api.22seven.com/heartbeat" , ScanCount = 10 },
                new Server() { Name = "TEST", Uri = "https://test.22seven.com/heartbeat" , ScanCount = 1 },
	        };
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Server> textSplitter)
        {
            textSplitter
                .Map(@"(what|whats)(ANYTHING)version(ANYTHING)"); 
        }
        
        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Server server)
        {
            
            await
                context.Respond(string.Format("I will have a look, give me a minute."));
            foreach (var link in _servers)
            {
                var list = new List<string>();

                for (int i = 0; i < link.ScanCount; i++)
                {
                    var restClient = new RestClient(link.Uri) { Timeout = 5000 };
                    try
                    {
                        var restResponse = await restClient.ExecuteGetTaskAsync(new RestRequest());
                        if (!restResponse.Content.Contains("fine.."))
                        {
                            await context.Respond(string.Format("Oops, seems that {0} is not fine.", link.Uri));
                            break;
                        }
                        var line = Regex.Match(restResponse.Content, @"(.*?),(.*?),(.*?),(.*?)!");
                        var serverName = line.Groups[2].Value.Trim();
                        var version = line.Groups[3].Value.Trim();
                        var date = DateTime.Now - DateTime.Parse(line.Groups[4].Value.Trim());
                        if (!list.Contains(serverName))
                        {
                            list.Add(serverName);
                            await context.Respond(string.Format("{0} is on version {1}, released {2} ago.", serverName, version, date.Humanize()));
                        }
                    }
                    catch (Exception)
                    {
                        context.Respond(string.Format("Oops, seems that {0} is offline.", link.Uri));
                        break;
                    }

                }

            }
            await context.Respond("Those are all the version that I could find.");
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "what version are we on",Description = "Returns the server versions."};
        }

        #endregion

        public class Server
        {
            public string Name { get; set; }
            public string Uri { get; set; }
            public bool IsTestServer { get; set; }
            public int ScanCount { get; set; }
        }
    }

    
}