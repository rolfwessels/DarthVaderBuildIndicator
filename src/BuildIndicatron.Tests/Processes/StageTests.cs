using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.Composition;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using log4net;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class StageTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private Stage _stage;
		private Mock<ISequencePlayer> _mockISequencePlayer;

		#region Setup/Teardown

		public void Setup()
		{
			_mockISequencePlayer = new Mock<ISequencePlayer>();
			
			
			_stage = new Stage(new SequencesFactory(), _mockISequencePlayer.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_mockISequencePlayer.VerifyAll();
		}

		#endregion

		[Test]
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			_stage.Should().NotBeNull();
		}

		[Test]
		public void Enqueue_GivenTextToSpeech_ShouldAddItemToQueueToPlay()
		{
			// arrange
			Setup();
			// action
			_stage.Enqueue(new SequencesText2Speech().ToDynamic());
			// assert
			_stage.Count.Should().Be(1);
		}

		[Test]
		public void Play_WhenCalled_ShouldStartThread()
		{
			// arrange
			Setup();
			_stage.Enqueue(new SequencesGpIo(1,true));
			_stage.Enqueue(new SequencesGpIo(2,true));
			// action
			_stage.Play().Wait();
			// assert
			_stage.Count.Should().Be(0);
		}
			
		[Test]
		public void Play_WhenCalled_ShouldCallPlayerOnAllSequencesGpIo()
		{
			// arrange
			Setup();
			_stage.Enqueue(new SequencesGpIo(1,true));
			_stage.Enqueue(new SequencesGpIo(2,true));
			// action
			_stage.Play().Wait();
			// assert
			_mockISequencePlayer.Verify(mc => mc.Play(It.IsAny<SequencesGpIo>()),Times.Exactly(2));
		}


		[Test]
		public void Play_WhenCalledTwice_ShouldReturnSameTask()
		{
			// arrange
			Setup();
			_stage.Enqueue(new SequencesGpIo(1, true) { BeginTime = 50 });
			_stage.Enqueue(new SequencesGpIo(2, true) { BeginTime = 50});
			// action
			var play = _stage.Play();
			// assert
			play.Should().Be(_stage.Play());
		}


		[Test]
		public void Play_WhenCalledTwiceThenOnceAgain_ShouldCreateNewTask()
		{
			// arrange
			Setup();
			_stage.Enqueue(new SequencesGpIo(1, true) { BeginTime = 10 });
			_stage.Enqueue(new SequencesGpIo(2, true) { BeginTime = 10 });
			_stage.Play();
			var play = _stage.Play();
			play.Wait();
			_stage.Enqueue(new SequencesGpIo(2, true) { BeginTime = 10 });
			// action
			var play1 = _stage.Play();
			// assert
			play.Should().NotBe(play1);
		}



		 
	}

	
}