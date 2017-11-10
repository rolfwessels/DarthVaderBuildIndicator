using System.Threading.Tasks;
using BuildIndicatron.Core.Api.Model;

namespace BuildIndicatron.Core.Api
{
    public interface IJenkensApi
    {
        Task<JenkensProjectsResult> GetAllProjects();
        Task<JenkensProjectsResult> BuildProject(string url);
        Task<JenkensProjectsResult> BuildProject(string url, JenkensProjectsBuildRequest param);
        Task<CrumbResult> GetCrumb();
        string Url { get; }
    }
}