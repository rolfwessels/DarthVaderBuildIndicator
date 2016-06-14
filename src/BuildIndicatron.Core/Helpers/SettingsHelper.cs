using System;
using System.Collections.Generic;
using System.Linq;
using BuildIndicatron.Core.Api.Model;
using BuildIndicatron.Core.Settings;

namespace BuildIndicatron.Core.Helpers
{
    static internal class SettingsHelper
    {
        public static string[] GetMyBuildingJobs(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("jenkins_monitor_builds", "builds").Split(',');
        }

        public static string GetBuildChannel(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("jenkins_monitor_channel", "#builds");
        }

        public static string[] GetProdBuilds(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("deployer_prod_builds", "ProjectName").Split(',');
        }

        public static string[] GetStagingBuilds(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("deployer_staging_builds", "ProjectName").Split(',');
        }
  
        public static string GetDefaultProxy(this ISettingsManager settingsManager)
        {
          return settingsManager.Get("default_proxy", "");
        }

        public static IEnumerable<Job> GetMyBuildingJobs(this ISettingsManager settingsmanager, JenkensProjectsResult allProjects)
        {
            var myBuildingJobs = GetMyBuildingJobs(settingsmanager);
            return allProjects.Jobs.Where(
                x => myBuildingJobs.Any(b => x.Name.Equals(b, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}