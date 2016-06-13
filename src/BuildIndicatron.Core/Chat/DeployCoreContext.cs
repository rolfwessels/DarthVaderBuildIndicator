using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using BuildIndicatron.Core.SimpleTextSplit;

namespace BuildIndicatron.Core.Chat
{
   
    public class DeployCoreContext : TextSplitterContextBase<DeployCoreContext.Meta>, IWithHelpText
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IJenkensApi _jenkensApi;


        public DeployCoreContext(ISettingsManager settingsManager, IJenkinsFactory jenkinsFactory)
        {
            _settingsManager = settingsManager;
            _jenkensApi = jenkinsFactory.GetDeployer();
        }

       
        #region Implementation of IReposonseFlow

        protected override void Apply(TextSplitter<Meta> textSplitter)
        {
            textSplitter
                .Map(@"deploy (?<State>WORD)")
                .Map(@"deploy"); 
        }

        protected override async Task Response(ChatContextHolder chatContextHolder, IMessageContext context, Meta server)
        {
            var jenkensProjectsResult = await _jenkensApi.GetAllProjects();

           
            server.State = server.State ?? States.Staging;
            var missingProject = await EnsureCorrectDeployProjects(context, server, jenkensProjectsResult);
            if (missingProject) return;

            switch (server.State)
            {
                case States.Staging:
                    await context.Respond("Starting the staging builds.");
                    foreach (var jobName in server.StagingBuilds )
                    {
                        var isDeployed = await Deploy(context, jenkensProjectsResult, jobName);
                        if (!isDeployed) return;
                    }
                    await context.Respond("Let me know if I can `monitor the staging versions` .");
                    break;
                case States.Prod:
                    await context.Respond("Starting the Prod builds.");
                    foreach (var jobName in server.ProdBuild)
                    {
                        var isDeployed = await Deploy(context, jenkensProjectsResult, jobName);
                        if (!isDeployed) return;
                    }
                    await context.Respond("Let me know if I can `monitor the production versions` .");
                    break;
                    
                default:
                    await context.Respond(string.Format("Oops, I dont know that state '{0}'.", server.State));
                    break;
            }
            
        }

        private async Task<bool> EnsureCorrectDeployProjects(IMessageContext context, Meta server,
            JenkensProjectsResult jenkensProjectsResult)
        {
            server.StagingBuilds = _settingsManager.GetStagingBuilds();
            server.ProdBuild = _settingsManager.GetProdBuilds();


            var missingProject =
                await EnsureValidProjects(context, jenkensProjectsResult, server.StagingBuilds, "deployer_staging_builds", "Staging");
            missingProject = missingProject ||
                             await
                                 EnsureValidProjects(context, jenkensProjectsResult, server.ProdBuild, "deployer_prod_builds",
                                     "Prod");
            return missingProject;
        }

        private async Task<bool> Deploy(IMessageContext context, JenkensProjectsResult jenkensProjectsResult, string jobName)
        {
            var jenkinsJob = jenkensProjectsResult.Jobs.First(job => IsMatch(job, jobName));
            await context.Respond(string.Format("*{0}* - status {1}", jenkinsJob.Name, jenkinsJob.Color));
            if (jenkinsJob.IsProcessing())
            {
                await context.Respond("Oops, looks like this job is already running. Wait for it to stop before continuing.");
                return false;
            }
            await _jenkensApi.BuildProject(jenkinsJob.Url);
            await context.Respond("Waiting for the job to start.");
            jenkinsJob = await WaitFor(jobName, result => result.IsProcessing(), TimeSpan.FromMinutes(2));
            if (!jenkinsJob.IsProcessing())
            {
                await context.Respond("Oops, looks like this did not start in time.");
                return false;
            }
            await context.Respond("Waiting for the job to finish.");
            jenkinsJob = await WaitFor(jobName, result => !result.IsProcessing(), TimeSpan.FromMinutes(_settingsManager.Get("build_processing_timeout_minutes",10)));
            if (jenkinsJob.IsProcessing())
            {
                await context.Respond("Oops, looks like this did not finish in time.");
                return false;
            }
            return true;
        }

        private async Task<Job> WaitFor(string jobName, Func<Job, bool> func, TimeSpan fromMinutes)
        {
            return await
                _jenkensApi.AwaitAsync(x => x.GetAllProjects().Result.Jobs.First(job => IsMatch(job, jobName)),
                    func, (int)fromMinutes.TotalMilliseconds, 5000);
        }

        private static bool IsMatch(Job job , string jobName)
        {
            return job.Name.ToLower() == jobName.Trim().ToLower();
        }

        private static async Task<bool> EnsureValidProjects(IMessageContext context,
            JenkensProjectsResult jenkensProjectsResult, string[] build, string configName, string prod)
        {
            var notFound =
                build.Where(jobName => jenkensProjectsResult.Jobs.All(job => !IsMatch(job,jobName)))
                    .ToArray();
            if (notFound.Any())
            {
                foreach (var jobName in notFound.Take(1))
                {
                    await
                        context.Respond(
                            string.Format(
                                "Please specify a project name for {3} deploy, '{0}' could not be found. You can change this by typing `set settings {1} {2}`",
                                jobName, configName, jenkensProjectsResult.Jobs.Random().Name, prod));
                }
            }
            return notFound.Any();
        }

        #endregion

        #region Implementation of IWithHelpText

        public IEnumerable<HelpMessage> GetHelp()
        {
            yield return new HelpMessage() {Call = "set setting [key] [value]",Description = "Set some settings."};
        }

        #endregion

        public class Meta
        {
            public string[] StagingBuilds { get; set; }
            public string[] ProdBuild { get; set; }
            public string State { get; set; }
        }

        public class States
        {
            public const string Staging = "staging";
            public const string Prod = "prod";
        }
        
    }

    
}