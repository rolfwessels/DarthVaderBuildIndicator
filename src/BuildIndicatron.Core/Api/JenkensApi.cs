using System.Reflection;
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
    public class JenkensApi : ApiBase, IJenkensApi
    {
        public JenkensApi(string hostApi = "http://therig:9999", string jenkenUsername = null, string jenkenPassword = null)
            : base(hostApi, jenkenUsername, jenkenPassword)
        {
            Url = hostApi;
        }
        
        public Task<JenkensProjectsResult> GetAllProjects()
        {
            var restRequest = GetRestRequest("api/json", Method.GET);
            //restRequest.AddParameter("pretty", "true");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddParameter("depth", "2");
            restRequest.AddParameter("tree", "jobs[name,color,healthReport[score],builds[duration,result],lastFailedBuild[number,timestamp,changeSet[items[author[fullName]]]]]");
            return ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }

        public string Url { get; private set; }
    }

}