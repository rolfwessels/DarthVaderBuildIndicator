using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using Raspberry.IO.GeneralPurpose;

namespace BuildIndicatron.Core.Chat
{
    

    public class LightsContext : ReposonseFlowBase, IReposonseFlow
    {
        private readonly IPinManager _pinManager;

        public LightsContext(IPinManager pinManager)
        {
            _pinManager = pinManager;
        }

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && StartsWith(context, "say"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            //var extractStartsWith = ExtractStartsWith(context, "say");
            _pinManager.SetPin(PinName.MainLightBlue,true);
            return context.Respond("test");
        }

        #endregion
    }
}