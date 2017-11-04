using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Api.Controllers;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Server.Tests.Controller
{
	[TestFixture]
	public class TextToSpeechControllerTests
	{

		private TextToSpeechController _textToSpeechController;
		private Mock<ITextToSpeech> _mockITextToSpeech;
		private Mock<IVoiceEnhancer> _mockIVoiceEnhancer;

		#region Setup/Teardown

		public void Setup()
		{
			_mockITextToSpeech = new Mock<ITextToSpeech>(MockBehavior.Strict);
			_mockIVoiceEnhancer = new Mock<IVoiceEnhancer>(MockBehavior.Strict);
			
			
			_textToSpeechController = new TextToSpeechController(_mockITextToSpeech.Object, _mockIVoiceEnhancer.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_mockITextToSpeech.VerifyAll();
			_mockIVoiceEnhancer.VerifyAll();
		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_textToSpeechController.Should().NotBeNull();
		}


		[Test]
		public void Get_GivenClean_ResultInFolderData()
		{
			// arrange
			Setup();
			_mockITextToSpeech.Setup(x => x.Play("Hello"));
			// action
			var playMp3FileResponse = _textToSpeechController.Get("Hello");
			// assert
			playMp3FileResponse.Should().NotBeNull();
			
		}




	}
}