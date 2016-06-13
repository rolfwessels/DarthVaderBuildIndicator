namespace BuildIndicatron.Core.Api
{
    public interface IJenkinsFactory
    {
        IJenkensApi GetDeployer();
        IJenkensApi GetBuilder();
    }

}