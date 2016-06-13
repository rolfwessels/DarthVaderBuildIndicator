using BuildIndicatron.Core.Settings;

namespace BuildIndicatron.Core.Helpers
{
    static internal class SettingsHelper
    {
        public static string[] MyBuildingJobs(this ISettingsManager settingsManager)
        {
            var jobNames = settingsManager.Get("jenkins_monitor_builds", "builds").Split(',');
            return jobNames;
        }

        public static string[] GetProdBuilds(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("deployer_prod_builds", "ProjectName").Split(',');
        }

        public static string[] GetStagingBuilds(this ISettingsManager settingsManager)
        {
            return settingsManager.Get("deployer_staging_builds", "ProjectName").Split(',');
        }
    }
}