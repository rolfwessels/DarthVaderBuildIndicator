using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using FluentAssertions;
using log4net;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Processes
{
	[TestFixture]
	public class GoogleTextToSpeachTests
	{
		public const string _longString = "This is some very long text , that just goes on and on. and on. and on. and on. and on. and on. and on. and on. Wow that was boring !";
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private GoogleTextToSpeach _googleTextToSpeach;
		private Mock<IDownloadToFile> _mockIDownloadToFile;
		private Mock<IMp3Player> _mockIMp3Player;

		#region Setup/Teardown

		public void Setup()
		{
			_mockIDownloadToFile = new Mock<IDownloadToFile>(MockBehavior.Strict);
			_mockIMp3Player = new Mock<IMp3Player>(MockBehavior.Strict);	
			_googleTextToSpeach = new GoogleTextToSpeach(_mockIDownloadToFile.Object, _mockIMp3Player.Object);
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
            _googleTextToSpeach.Play("your mother").Wait();
			// assert
		}
		
		[Test]
		public void Play_GivenAVeryLongString_ShouldCallDownload()
		{
			// arrange
			Setup();
			_mockIDownloadToFile.Setup(mc => mc.DownloadToTempFile(It.IsAny<Uri>(), It.IsAny<string>())).Returns("file.file");
            _mockIMp3Player.Setup(mc => mc.PlayFile("file.file")).Returns(Task.FromResult(true));
			// action
			_googleTextToSpeach.Play(_longString).Wait();
			// assert
			_mockIDownloadToFile.Verify(mc => mc.DownloadToTempFile(It.IsAny<Uri>(), It.IsAny<string>()),Times.Exactly(3));
		}

		[Test]
		public void Split_GivenAVeryShort_ShouldCallDownload()
		{
			// arrange
			Setup();
			// action
			var split = _googleTextToSpeach.Split("Short string");
			// assert
			split.Should().ContainInOrder(new[] { "Short string" });
		}


		[Test]
		public void Split_GivenALongerString_ShouldCallDownload()
		{
			// arrange
			Setup();
			Console.Out.WriteLine("This is some very long text , that just goes on and on. and on.".Length);
			// action
			var split = _googleTextToSpeach.Split("This is some very long text , that just goes on and on. and on. This is some very long text , that just goes on and on. and on.");
			// assert
			split.Should().ContainInOrder(new[] 
			{ "This is some very long text , that just goes on and on. and on.",
				"This is some very long text , that just goes on and on. and on."}
				);
		}

		[Test]
		public void Split_GivenAVeryLongString_ShouldCallDownload()
		{
			// arrange
			Setup();
		
			// action
			var split = _googleTextToSpeach.Split(_longString);
			// assert
			split.Count().Should().Be(3);
		}

		[Test]
		[Explicit]
		public void Play_GivenFullImplementation_ShouldPlayFile()
		{
			Setup();
			// arrange
			_googleTextToSpeach = new GoogleTextToSpeach(new DownloadToFile("./"), new Mp3Player());
			// action
			_googleTextToSpeach.Play(_longString);
		}


	}
}