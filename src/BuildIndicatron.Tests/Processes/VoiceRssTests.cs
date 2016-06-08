using System;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using log4net;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class VoiceRssTests
	{
		public const string _longString = "This is some very long text , that just goes on and on. and on. and on. and on. and on. and on. and on. and on. Wow that was boring !";
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private VoiceRss _voiceRss;
		private Mock<IDownloadToFile> _mockIDownloadToFile;
		private Mock<IMp3Player> _mockIMp3Player;

		#region Setup/Teardown

		public void Setup()
		{
			_mockIDownloadToFile = new Mock<IDownloadToFile>(MockBehavior.Strict);
			_mockIMp3Player = new Mock<IMp3Player>(MockBehavior.Strict);	
			_voiceRss = new VoiceRss(_mockIDownloadToFile.Object, _mockIMp3Player.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_mockIMp3Player.VerifyAll();
			_mockIDownloadToFile.VerifyAll();
		}

		#endregion

		[Test]
		public void Play_GivenString_ShouldCallDownload()
		{
			// arrange
			Setup();
			_mockIDownloadToFile.Setup(mc => mc.DownloadToTempFile(It.IsAny<Uri>(), "your mother"))
			 .Returns("file.file");
            _mockIMp3Player.Setup(mc => mc.PlayFile("file.file")).Returns(Task.FromResult(true)); ;
			// action
            _voiceRss.Play("your mother").Wait();
			// assert
		}
		
		[Test]
		public void Play_GivenAVeryLongString_ShouldCallDownload()
		{
			// arrange
			Setup();
			_mockIDownloadToFile.Setup(mc => mc.DownloadToTempFile(It.IsAny<Uri>(), It.IsAny<string>())).Returns("file.file");
            _mockIMp3Player.Setup(mc => mc.PlayFile("file.file")).Returns(Task.FromResult(true)); ;
			// action
			_voiceRss.Play(_longString).Wait();
			// assert
			_mockIDownloadToFile.Verify(mc => mc.DownloadToTempFile(It.IsAny<Uri>(), It.IsAny<string>()),Times.Exactly(1));
		}

		[Test]
		[Explicit]
		public void Play_GivenFullImplementation_ShouldPlayFile()
		{
			Setup();
			// arrange
			_voiceRss = new VoiceRss(new DownloadToFile("./"), new Mp3Player());
			// action
			_voiceRss.Play("Luke why am i not your father.");
		}


	}
}