using System.Threading.Tasks;
using BuildIndicatron.Core.Api.Model;
using RestSharp;

namespace BuildIndicatron.Core.Api
{
    /// <summary>
    /// </summary>
    public class JenkensApi : ApiBase
    {
        public JenkensApi(string hostApi = "http://fulliautomatix:8080") : base(hostApi)
        {
            
        }

        public Task<JenkensProjectsResult> GetAllProjects()
        {
            var restRequest = GetRestRequest("api/json", Method.GET);
            //restRequest.AddParameter("pretty", "true");
            restRequest.AddParameter("depth", "1");
            restRequest.AddParameter("tree", "jobs[name,color,healthReport[score],lastFailedBuild[number,timestamp,changeSet[items[author[fullName]]]]]");
            return ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }
    }

}