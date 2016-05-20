using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public interface IMessageContext
    {
        string Text { get;  }
        bool IsDirectedAtMe { get; }
        Task Respond(string message);
    }
}