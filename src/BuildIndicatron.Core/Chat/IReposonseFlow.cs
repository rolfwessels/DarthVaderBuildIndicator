using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public interface IReposonseFlow
    {
        Task<bool> CanRespond(IMessageContext context);
        Task Respond(ChatContextHolder chatContextHolder, IMessageContext context);
    }
}