using System.Reflection;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using Newtonsoft.Json;
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
            restRequest.AddParameter("tree", "jobs[name,color,url,healthReport[score],builds[duration,result],lastFailedBuild[number,timestamp,changeSet[items[author[fullName]]]]]");
            return ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }

        public string Url { get; private set; }

        public async Task<JenkensProjectsResult> BuildProject(string url)
        {
            var crumbResult = await GetCrumb();

            var restRequest = GetRestRequest(url.Replace(Url, "") + "/build", Method.POST);
            restRequest.AddHeader("Jenkins-Crumb", crumbResult.Crumb);
            restRequest.RequestFormat = DataFormat.Json;
            return await ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }

        public async Task<JenkensProjectsResult> BuildProject(string url,JenkensProjectsBuildRequest param)
        {
            var crumbResult = await GetCrumb();
            var restRequest = GetRestRequest(url.Replace(Url, "") + "/build", Method.POST);
            restRequest.AddHeader("Jenkins-Crumb", crumbResult.Crumb);
            restRequest.AddParameter("json", JsonConvert.SerializeObject(param));
            restRequest.RequestFormat = DataFormat.Json;
            return await ProcessDefaultRequest<JenkensProjectsResult>(restRequest);
        }

        public Task<CrumbResult> GetCrumb()
        {
            var request = GetRestRequest("crumbIssuer/api/json",Method.GET);
            return ProcessDefaultRequest<CrumbResult>(request);
        }

        public static JenkensApi OnJenkinsDeloyer(ISettingsManager resolve)
        {
            return new JenkensApi(
                resolve.Get("jenkins_deployer_host", "http://therig1231:9999"),
                resolve.Get("jenkins_deployer_user",null),
                resolve.Get("jenkins_deployer_password",null)
                );
        }

        public static JenkensApi GetJenkins(ISettingsManager resolve)
        {
            return new JenkensApi(
                resolve.Get("jenkins_host", "http://therig:9999"),
                resolve.Get("jenkins_user",null),
                resolve.Get("jenkins_password",null)
                );
        }
    }

}