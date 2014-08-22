using System.Reflection;
using BuildIndicatron.Core.Processes;
using NUnit.Framework;
using FluentAssertions;
using log4net;

namespace BuildIndicatron.Server.Process
{
	[TestFixture]
	public class Mp3PlayerTestsTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private Mp3Player _mp3PlayerTests;

		#region Setup/Teardown

		public void Setup()
		{
			_mp3PlayerTests = new Mp3Player();
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
			_mp3PlayerTests.Should().NotBeNull();
		}

		 
		[Test]
		[Explicit]
		public void PlayFile_WhenWav_ShouldPlay()
		{
			// arrange
			Setup();
			// action 
			_mp3PlayerTests.PlayFile(@"Resources\darthvader_yesmaster.wav");
			// assert
			_mp3PlayerTests.Should().NotBeNull();
		}
 
		[Test]
		[Explicit]
		public void PlayFile_WhenGivenMp3_ShouldPlay()
		{
			// arrange
			Setup();
			// action 
			_mp3PlayerTests.PlayFile(@"Resources\force.mp3");
			// assert
			_mp3PlayerTests.Should().NotBeNull();
		}

		 
	}

}