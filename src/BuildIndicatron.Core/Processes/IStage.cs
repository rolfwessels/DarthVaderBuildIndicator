using System.Threading.Tasks;
using BuildIndicatron.Shared.Models.Composition;

namespace BuildIndicatron.Core.Processes
{
    public interface IStage
    {
        void Enqueue(dynamic sequencese);
        void Enqueue(Sequences sequencese);
        int Count { get; }
        Task Play();
    }
}