using System.Threading.Tasks;
using BuildIndicatron.Core.Api.Model;

namespace BuildIndicatron.Core.Api
{
    public interface IJenkensApi
    {
        Task<JenkensProjectsResult> GetAllProjects();
        string Url { get; }
    }
}