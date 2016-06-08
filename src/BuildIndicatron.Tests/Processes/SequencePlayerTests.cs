using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.Composition;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class SequencePlayerTestsTests
	{

		private SequencePlayer _sequencePlayerTests;
		private Mock<ITextToSpeech> _mockITextToSpeech;
		private Mock<IVoiceEnhancer> _mockIVoiceEnhancer;
		private Mock<IMp3Player> _mockIMp3Player;
		private Mock<ISoundFilePicker> _mockISoundFilePicker;
		private Mock<IPinManager> _mockIPinManager;

		#region Setup/Teardown

		public void Setup()
		{
			_mockITextToSpeech = new Mock<ITextToSpeech>(MockBehavior.Strict);
			_mockIVoiceEnhancer = new Mock<IVoiceEnhancer>(MockBehavior.Strict);
			_mockIMp3Player = new Mock<IMp3Player>(MockBehavior.Strict);
			_mockISoundFilePicker = new Mock<ISoundFilePicker>(MockBehavior.Strict);
			_mockIPinManager = new Mock<IPinManager>(MockBehavior.Strict);
			
			

			_sequencePlayerTests = new SequencePlayer(_mockITextToSpeech.Object, _mockIMp3Player.Object, _mockIVoiceEnhancer.Object, _mockISoundFilePicker.Object, _mockIPinManager.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_mockITextToSpeech.VerifyAll();
			_mockIMp3Player.VerifyAll();
			_mockIVoiceEnhancer.VerifyAll();
			_mockISoundFilePicker.VerifyAll();
			_mockIPinManager.VerifyAll();
		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_sequencePlayerTests.Should().NotBeNull();
		}

		[Test]
		public void Play_GivenSequencesText2Speech_ShouldSequencesText2Speech()
		{
			// arrange
			Setup();
            _mockITextToSpeech.Setup(mc => mc.Play("Help", _mockIVoiceEnhancer.Object)).Returns(Task.FromResult(true));
			var sequences = new SequencesText2Speech() { Text = "Help"};
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}

		[Test]
		public void Play_GivenSequencesText2SpeechWithDisabledVoiceEnhancement_ShouldSequencesText2Speech()
		{
			// arrange
			Setup();
            _mockITextToSpeech.Setup(mc => mc.Play("Help")).Returns(Task.FromResult(true));
			var sequences = new SequencesText2Speech() { Text = "Help" ,DisableTransform = true};
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}


		[Test]
		public void Play_GivenSequencesGpIo_ShouldSequencesGpIo()
		{
			// arrange
			Setup();
			_mockIPinManager.Setup(mc => mc.SetPin(123,true));
			var sequences = new SequencesGpIo() { IsOn = true, Pin = 123} ;
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}

		[Test]
		public void Play_GivenSequencesGpIoAndTarget_ShouldSequencesGpIo()
		{
			// arrange
			Setup();
			_mockIPinManager.Setup(mc => mc.SetPin(PinName.MainLightGreen, true));
			var sequences = new SequencesGpIo() { IsOn = true, PinName = PinName.MainLightGreen} ;
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}

		[Test]
		public void Play_GivenSequencesInsult_ShouldSequencesInsult()
		{
			// arrange
			Setup();
			var sequences = new SequencesInsult();
            _mockITextToSpeech.Setup(mc => mc.Play(It.IsAny<string>(), _mockIVoiceEnhancer.Object)).Returns(Task.FromResult(true));
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
			
		}
		[Test]
		public void Play_GivenSequencesOneLiner_ShouldSequencesOneLiner()
		{
			// arrange
			Setup();
			_mockITextToSpeech.Setup(mc => mc.Play(It.IsAny<string>(), _mockIVoiceEnhancer.Object)).Returns(Task.FromResult(true));
			var sequences = new SequencesOneLiner();
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}
		[Test]
		public void Play_GivenSequencesPlaySound_ShouldSequencesPlaySound()
		{
			// arrange
			Setup();
			_mockISoundFilePicker.Setup(mc => mc.PickFile("Test"))
			 .Returns("file");
			_mockIMp3Player.Setup(mc => mc.PlayFile("file"));
			var sequences = new SequencesPlaySound() { File = "Test" };
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}
		[Test]
		public void Play_GivenSequencesQuotes_ShouldSequencesQuotes()
		{
			// arrange
			Setup();
			var sequences = new SequencesQuotes();
            _mockITextToSpeech.Setup(mc => mc.Play(It.IsAny<string>(), _mockIVoiceEnhancer.Object)).Returns(Task.FromResult(true));
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}
		[Test]
		public void Play_GivenSequencesTweet_ShouldSequencesTweet()
		{
			// arrange
			Setup();
			var sequences = new SequencesTweet();
			// action
			_sequencePlayerTests.Play(sequences);
			// assert
		}





	}

}