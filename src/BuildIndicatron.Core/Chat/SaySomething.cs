using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;

namespace BuildIndicatron.Core.Chat
{
    

    public class SaySomething : ReposonseFlowBase, IReposonseFlow
    {
        private readonly IMp3Player _mp3Player;
        private readonly ISoundFilePicker _soundFilePicker;

        public SaySomething(IMp3Player mp3Player, ISoundFilePicker soundFilePicker)
        {
            _mp3Player = mp3Player;
            _soundFilePicker = soundFilePicker;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "say something"));
        }

        public async Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            var extractStartsWith = ExtractStartsWith(context, "say something");
            await _mp3Player.PlayFile(_soundFilePicker.PickFile(extractStartsWith));
            
        }

        #endregion
    }
}