using NUnit.Framework;

namespace BuildIndicatron.Tests.IntegrationTests
{
    /// <summary>
    /// </summary>
    [TestFixture]
    public class RemoteIntegrationTest : LocalIntegrationTests
    {
        public RemoteIntegrationTest()
        {
            _hostApi = "http://192.168.1.14:5000/";
        }

    }
}