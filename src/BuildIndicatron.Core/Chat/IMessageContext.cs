using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public interface IMessageContext
    {
        string Text { get; }
        bool IsDirectedAtMe { get; }
        bool IsBotMessage { get; }
        string FromChatHub { get; }
        string FromUser { get; }
        Task Respond(string message);
    }
}