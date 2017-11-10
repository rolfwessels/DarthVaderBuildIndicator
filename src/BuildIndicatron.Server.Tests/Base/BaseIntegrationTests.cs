using System.Reflection;
using BuildIndicatron.Core.Api;
using log4net;

namespace BuildIndicatron.Server.Tests.Base
{
    public class BaseIntegrationTests
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected static RobotApi BuildIndicatorApi;

        protected static RobotApi CreateClient(string baseUri)
        {
            BuildIndicatorApi = new RobotApi(baseUri);
            return BuildIndicatorApi;
        }
    }
}