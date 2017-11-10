using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Api.Controllers;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Server.Tests.Controller
{
	[TestFixture]
	public class SoundPlayerControllerTests
	{

		private SoundPlayerController _soundPlayerController;
		private Mock<IMp3Player> _mockIMp3Player;

		#region Setup/Teardown

		public void Setup()
		{
			_mockIMp3Player = new Mock<IMp3Player>(MockBehavior.Strict);
			_soundPlayerController = new SoundPlayerController(_mockIMp3Player.Object, new SoundFilePicker(Settings.Default.SoundFileLocation));
		}

		[TearDown]
		public void TearDown()
		{
			_mockIMp3Player.VerifyAll();
		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_soundPlayerController.Should().NotBeNull();
		}


		[Test]
		public void Get_GivenClean_ResultInFolderData()
		{
			// arrange
			Setup();
			
			// action
			var playMp3FileResponse = _soundPlayerController.Get();
			// assert
			playMp3FileResponse.Should().NotBeNull();
			playMp3FileResponse.Folders.Count.Should().BeGreaterOrEqualTo(1);
		}


		[Test]
		public void Get_GivenFileThatDoesExist_ResultPlayFile()
		{
			// arrange
			Setup();
			_mockIMp3Player.Setup(mc => mc.PlayFile(It.IsAny<string>())).Returns(Task.FromResult("ample"));
			// action
			var playMp3FileResponse = _soundPlayerController.Get(@"Start\Force.mp3");
			// assert
			playMp3FileResponse.Should().NotBeNull();
		}

		[Test]
		public void Get_GivenFileThatHasPath_ResultPlayFile()
		{
			// arrange
			Setup();
            _mockIMp3Player.Setup(mc => mc.PlayFile(It.IsAny<string>())).Returns(Task.FromResult("ample")); ;
			// action
			var playMp3FileResponse = _soundPlayerController.Get("Start","Force.mp3");
			// assert
			playMp3FileResponse.Should().NotBeNull();
		}


		[Test]
		public void Get_GivenFileThatDoesExist_ResultRandomFile()
		{
			// arrange
			Setup();
            _mockIMp3Player.Setup(mc => mc.PlayFile(It.IsAny<string>())).Returns(Task.FromResult("ample")); ;
			// action
			var playMp3FileResponse = _soundPlayerController.Get(@"Start");
			// assert
			playMp3FileResponse.Should().NotBeNull();
		}



	}
}