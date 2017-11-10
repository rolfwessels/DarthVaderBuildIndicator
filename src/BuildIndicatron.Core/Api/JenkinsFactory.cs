namespace BuildIndicatron.Core.Api
{
    public class JenkinsFactory : IJenkinsFactory
    {
        private readonly IJenkinsISettings _settings;

        public JenkinsFactory(IJenkinsISettings settings)
        {
            _settings = settings;
        }

        #region Implementation of IJenkinsFactory

        public IJenkensApi GetDeployer()
        {
            return GetBuilder();
        }


        public IJenkensApi GetBuilder()
        {
            return new JenkensApi(_settings.JenkinsHost, _settings.JenkinsUser,
                _settings.JenkinsPassword);
        }

        #endregion
    }
}