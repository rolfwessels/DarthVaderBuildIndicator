using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;

namespace BuildIndicatron.Core.Chat
{
    public class SetIoContext : ReposonseFlowBase, IReposonseFlow, IFlowHelp
    {
        private readonly LightPin[] _lights;
        private readonly IPinManager _pinManager;

        public SetIoContext(IPinManager pinManager)
        {
            _pinManager = pinManager;
            _lights = new[]
            {
                new LightPin("Main", "Green", PinName.MainLightGreen),
                new LightPin("Main", "Red", PinName.MainLightRed),
                new LightPin("Main", "Blue", PinName.MainLightBlue),
                new LightPin("Secondary", "Green", PinName.SecondaryLightGreen),
                new LightPin("Secondary", "Red", PinName.SecondaryLightRed),
                new LightPin("Secondary", "Blue", PinName.SecondaryLightBlue)
            };
        }

        #region Nested type: LightPin

        public class LightPin
        {
            private readonly string _color;
            private readonly PinName _pin;
            private readonly string _section;

            public LightPin(string section, string color, PinName pin)
            {
                _section = section.ToLower();
                _color = color.ToLower();
                _pin = pin;
            }

            public void Set(string text, IPinManager c)
            {
                text = text.ToLower();
                if (text.Contains(_section))
                {
                    c.SetPin(_pin, text.Contains(_color));
                }
            }
        }

        #endregion

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return Task.FromResult(IsDirectedAtMe(context) && ContainsText(context, "set"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            return Task.Run(() =>
            {
                foreach (LightPin pin in _lights)
                {
                    pin.Set(context.Text, _pinManager);
                }
                context.Respond("lights set");
            });
        }

        #endregion

        #region Implementation of IFlowHelp

        public string GetHelp()
        {
            return "'set' '{section,secondary}' light '{green,red,blue,off}'";
        }

        #endregion
    }
}