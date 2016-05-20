using System.Threading.Tasks;

namespace BuildIndicatron.Core.Chat
{
    public interface IChatBot
    {
        Task Process(IMessageContext context);
    }
}