using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Shared.Models.Composition;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class StageTests
	{

		private Stage _stage;

		#region Setup/Teardown

		public void Setup()
		{
			_stage = new Stage(new SequencesFactory());
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
		
		

		 
	}

	
}