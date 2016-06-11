using System.Collections.Generic;
using System.Threading.Tasks;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{

    public class SetSettingsContext : TextSplitterContextBase<SetSettingsContext.SettingChange>, IReposonseFlow, IWithHelpText
    {
        private readonly ISettingsManager _settingsContext;
        private TextSplitter<SettingChange> _textSplitter;


        public SetSettingsContext(ISettingsManager settingsContext)
        {
            _settingsContext = settingsContext;
        }

        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<SettingChange> textSplitter)
        {
            textSplitter
                .Map(@"set setting (?<key>WORD) (?<value>ANYTHING)")
                .Map(@"set setting (?<key>WORD)")
                .Map(@"set setting"); 
        }
        
        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, SettingChange settingChange)
        {
            if (string.IsNullOrEmpty(settingChange.Key))
            {
                await context.Respond("what is the key?");
                var quickTextSplitterContext = new QuickTextSplitterContext<SettingChange>(settingChange,
                    x => x.Map(@"(?<key>WORD)"),Response);
                chatContextHolder.AddOneTime(quickTextSplitterContext);
            }
            else if (string.IsNullOrEmpty(settingChange.Value))
            {
                await context.Respond("what is the value?");

                chatContextHolder.AddOneTime(new QuickTextSplitterContext<SettingChange>(settingChange,
                    x => x.Map(@"(?<value>ANYTHING)"), Response));
            }
            else
            {
                _settingsContext.Set(settingChange.Key, settingChange.Value);
                await
                    context.Respond(string.Format("setting *{0}* to *{1}*", settingChange.Key,
                        settingChange.Value));
            }
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "set setting [key] [value]",Description = "Settings some settings."};
        }

        #endregion

        public class SettingChange
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }

    
}