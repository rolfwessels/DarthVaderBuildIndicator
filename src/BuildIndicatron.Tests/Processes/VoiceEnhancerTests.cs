using System.Reflection;
using BuildIndicatron.Core.Processes;
using FluentAssertions;
using log4net;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class VoiceEnhancerTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private VoiceEnhancer _voiceEnhancer;

		#region Setup/Teardown

		public void Setup()
		{
			_voiceEnhancer = new VoiceEnhancer(@"Resources\Sounds\Start\Force.mp3", "speed 0.78 echo 0.8 0.88 6.0 0.4");
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
			_voiceEnhancer.Should().NotBeNull();
		}

		[Test]
		[Explicit]
		public void PlayFile_WhenWav_ShouldPlay()
		{
			// arrange
			Setup();
			// action 
			_voiceEnhancer.PlayFile(@"Resources\darthvader_yesmaster.wav");
			// assert
			
		}
 
	}

}