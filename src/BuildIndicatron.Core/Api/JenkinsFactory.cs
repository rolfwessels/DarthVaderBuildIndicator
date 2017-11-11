using System;

namespace BuildIndicatron.Core.Api
{
    public class JenkinsFactory : IJenkinsFactory
    {
        private readonly Lazy<JenkensApi> _lazyJenkinsInstance;

        public JenkinsFactory(IJenkinsISettings settings)
        {
            _lazyJenkinsInstance = new Lazy<JenkensApi>(() => new JenkensApi(settings.JenkinsHost, settings.JenkinsUser,
                settings.JenkinsPassword));
        }

        #region Implementation of IJenkinsFactory

        public IJenkensApi GetDeployer()
        {
            return GetBuilder();
        }


        public IJenkensApi GetBuilder()
        {
            return _lazyJenkinsInstance.Value;
        }

        #endregion
    }
}