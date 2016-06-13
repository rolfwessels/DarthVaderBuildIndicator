using BuildIndicatron.Core.Settings;

namespace BuildIndicatron.Core.Api
{
    public class JenkinsFactory : IJenkinsFactory
    {
        private readonly ISettingsManager _settingsManager;

        public JenkinsFactory(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region Implementation of IJenkinsFactory

        public IJenkensApi GetDeployer()
        {
            return JenkensApi.OnJenkinsDeloyer(_settingsManager);
        }


        public IJenkensApi GetBuilder()
        {
            return JenkensApi.GetJenkins(_settingsManager);
        }

        #endregion
    }
}