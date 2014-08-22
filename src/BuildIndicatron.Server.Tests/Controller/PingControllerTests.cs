using BuildIndicatron.Server.Controllers;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Server.Tests.Controller
{
	[TestFixture]
	public class PingControllerTests
	{

		private PingController _pingController;

		#region Setup/Teardown

		public void Setup()
		{
			_pingController = new PingController();
		}

		[TearDown]
		public void TearDown()
		{

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