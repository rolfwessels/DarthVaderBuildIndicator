using System.Threading.Tasks;

namespace BuildIndicatron.Core.Api
{
    public interface IMonitorJenkins
    {
        Task Check();
    }
}