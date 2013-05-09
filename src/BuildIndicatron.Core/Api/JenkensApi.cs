using BuildIndicatron.Core.Api.Model;
using RestSharp;

#if WINDOWS_PHONE
using BuildIndicatron.App.Core.Task;
#else
using log4net;
using System.Threading.Tasks;
#endif

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
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddParameter("depth", "1");
            restRequest.AddParameter("tree", "jobs[name,color,healthReport[score],build[duration,result],lastFailedBuild[number,timestamp,changeSet[items[author[fullName]]]]]");
            return ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }

    }

}