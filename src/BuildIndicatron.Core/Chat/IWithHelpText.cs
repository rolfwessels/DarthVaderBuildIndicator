using System.Collections.Generic;

namespace BuildIndicatron.Core.Chat
{
    public interface IWithHelpText
    {
        IEnumerable<HelpMessage> GetHelp();
    }
}