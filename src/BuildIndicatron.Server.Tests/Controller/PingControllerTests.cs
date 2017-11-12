using NUnit.Framework;
using FluentAssertions;
using BuildIndicatron.Server.Core.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace BuildIndicatron.Server.Tests.Controller
{
    [TestFixture]
    public class PingControllerTests
    {
        private PingController _pingController;
        private Mock<IHostingEnvironment> _mockIHostingEnvironment;

        #region Setup/Teardown

        public void Setup()
        {
            _mockIHostingEnvironment = new Mock<IHostingEnvironment>();
            _pingController = new PingController(_mockIHostingEnvironment.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockIHostingEnvironment.VerifyAll();
        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _pingController.Should().NotBeNull();
        }


        [Test]
        public void Get_WhenCalled_ShouldContainVersionNumber()
        {
            // arrange
            Setup();
            // action
            var pingResponse = _pingController.Get();
            // assert
            pingResponse.Version.Should().Be("1.0.0.0");
        }

        [Test]
        public void Get_WhenCalled_ShouldContainPlatform()
        {
            // arrange
            Setup();
            // action
            var pingResponse = _pingController.Get();
            // assert
            pingResponse.Platform.Should().Be("Win32NT");
        }
    }
}