using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;

namespace BuildIndicatron.Core.Chat
{
    public class SetIoContext : ReposonseFlowBase, IReposonseFlow, IWithHelpText
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

            public string Section
            {
                get { return _section; }
            }

            public string Color
            {
                get { return _color; }
            }

            public void Set(string text, IPinManager c)
            {
                if (ContainsSection(text))
                {
                    c.SetPin(_pin, IsOn(text));
                }
            }

            public bool IsOn(string text)
            {
                return text.ToLower().Contains(_color);
            }

            public bool ContainsSection(string text)
            {
                return text.ToLower().Contains(_section);
            }
        }

        #endregion

        #region Implementation of IReposonseFlow

        public Task<bool> CanRespond(IMessageContext context)
        {
            return
                Task.FromResult(IsDirectedAtMe(context) && ContainsText(context, "set") &&
                                ContainsText(context, "light"));
        }

        public Task Respond(ChatContextHolder chatContextHolder, IMessageContext context)
        {
            return Task.Run(() =>
            {
                foreach (LightPin pin in _lights)
                {
                    pin.Set(context.Text, _pinManager);
                }
                LightPin[] allOn = _lights.Where(x => x.ContainsSection(context.Text) && x.IsOn(context.Text)).ToArray();
                if (!allOn.Select(x => x.Color).Any())
                {
                    context.Respond(string.Format("{0} lights are now off",
                        _lights.Where(x => x.ContainsSection(context.Text))
                            .Select(x => x.Section)
                            .Distinct()
                            .StringJoin()));
                }
                else
                {
                    context.Respond(string.Format("{0} {1} lights are now on",
                        allOn.Select(x => x.Section).Distinct().StringJoin(),
                        allOn.Select(x => x.Color).Distinct().StringJoin()));
                }
            });
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return
                new HelpMessage
                {
                    Call = "set *{main,secondary}* light *{green,red,blue,off}*",
                    Description = "Allow robot light up."
                };
        }

        #endregion
    }
}