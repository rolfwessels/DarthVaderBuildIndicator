using System;
using System.IO;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Core.Settings;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Processes
{
    [TestFixture]
    public class DownloadToFileTests
    {
        private const string Text = "Luke i am your father";
        private const string FileName = @".\Luke_i_am_your.IUIGf1rGXLtHhur5wCrpg.mp3";
        private DownloadToFile _downloadToFile;
        private Mock<ISettingsManager> _mockISettingsManager;

        #region Setup/Teardown

        public void Setup()
        {
            _mockISettingsManager = new Mock<ISettingsManager>();


            _downloadToFile = new DownloadToFile("./", _mockISettingsManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockISettingsManager.VerifyAll();
        }

        #endregion

        [Test]
        public void DownloadToTempFile_GivenUri_ShouldDownloadTheFileOnlyOnce()
        {
            // arrange
            Setup();
            var uri = new Uri(string.Format(GoogleTextToSpeach.UriToDownload, Uri.EscapeUriString(Text)));
            if (File.Exists(FileName)) File.Delete(FileName);
            File.WriteAllText(FileName, "ff");
            // action
            var fileName = _downloadToFile.DownloadToTempFile(uri, Text);
            // assert
            File.ReadAllText(FileName).Should().Be("ff");
        }
        
        [Test]
        [Explicit]
        public void DownloadToTempFile_GivenUri_ShouldDownloadTheItemToAFile()
        {
            // arrange
            Setup();
            var uri = new Uri(string.Format(GoogleTextToSpeach.UriToDownload, Uri.EscapeUriString(Text)));
            if (File.Exists(FileName)) File.Delete(FileName);
            // action
            _downloadToFile.DownloadToTempFile(uri, Text);
            // assert
            File.Exists(FileName).Should().BeTrue();
        }

        [Test]
        public void GetFileName_GivenDescriptionText_ShouldCreateFileName()
        {
            // arrange
            Setup();
            // action
            var fileName = _downloadToFile.GetFileName("Luke i*am your father");
            // assert
            fileName.Should().StartWith("Luke_i_am_your.");
            fileName.Should().EndWith(".mp3");
        }

        [Test]
        public void GetFileName_GivenEmpty_ShouldShortString()
        {
            // arrange
            Setup();
            // action
            var fileName = _downloadToFile.GetFileName("");
            // assert
            fileName.Should().StartWith(".");
            fileName.Should().EndWith(".mp3");
            fileName.Length.Should().BeGreaterThan(5);
        }

        [Test]
        public void GetFileName_GivenShortString_ShouldShortString()
        {
            // arrange
            Setup();
            // action
            var fileName = _downloadToFile.GetFileName("Luke");
            // assert
            fileName.Should().Be("Luke.sh37FI0gsf692NhkFklwQ.mp3");
        }
    }
}