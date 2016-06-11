using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{

    public class GetSettingsContext : TextSplitterContextBase<GetSettingsContext.SettingChange>, IWithHelpText
    {
        private readonly ISettingsManager _settingsContext;
        
        public GetSettingsContext(ISettingsManager settingsContext)
        {
            _settingsContext = settingsContext;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<SettingChange> textSplitter)
        {
            textSplitter
                .Map(@"get (setting|settings) (?<key>WORD)")
                .Map(@"get (setting|settings)"); 
        }
        
        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, SettingChange server)
        {
            if (string.IsNullOrEmpty(server.Key))
            {
                await context.Respond("I have the following: ");
                foreach (var key in _settingsContext.Get().Keys)
                {
                    await context.Respond(key);
                }
                chatContextHolder.AddOneTime(new QuickTextSplitterContext<SettingChange>(server,
                    x => x.Map(@"(?<key>WORD)"), Response));
            }
            else
            {
                var s = _settingsContext.Get(server.Key);
                await
                    context.Respond(s);
            }
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "get setting [key]",Description = "Get some settings."};
        }

        #endregion

        public class SettingChange
        {
            public string Key { get; set; }
        }
    }

    
}