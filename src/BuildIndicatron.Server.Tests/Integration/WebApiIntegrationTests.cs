using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Server.Chip;
using BuildIndicatron.Server.Tests.Base;
using BuildIndicatron.Shared.Enums;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using log4net;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace BuildIndicatron.Server.Tests.Integration
{
    [TestFixture]
    [Category("Integration")]
    [Explicit]
    public class WebApiIntegrationTests : BaseIntegrationTests
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IDisposable _disposable;

        #region Setup/Teardown

        [OneTimeSetUp]
        public void SetupFixture()
        {
            var baseUri = string.Format("http://localhost:{0}/api", GetRandom.Int(19000, 19999));
            _log.Info(string.Format("Starting api on {0}", baseUri));

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(baseUri)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            host.Run();
            _disposable = host;
            _log.Info("Started");

            CreateClient(baseUri);

            this.WaitFor(t => EnsureConnected(), 10000);
        }

        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void GetClips_GivenCall_ResultShouldReturnAllClips()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.GetClips().Result;
            // assert
            result.Should().NotBeNull();
            result.Folders.Count.Should().BeGreaterOrEqualTo(1);
            result.Folders.Select(x => x.Name).Should().Contain("Start");
            result.Folders.SelectMany(x => x.Files).Should().Contain("darthvader_yesmaster.wav");
        }

        [Test]
        public void GpIoOutput_GivenPinId_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.GpIoOutput(12, true).Result;
            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GpIoOutput_GivenPinName_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.GpIoOutput(PinName.SecondaryLightBlue, true).Result;
            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Ping_GivenGet_ResultObjectContainingVersion()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.Ping().Result;
            // assert
            result.Version.Should().NotBeEmpty();
            result.Platform.Should().NotBeEmpty();
        }

        [Test]
        public void PlayMp3File_GivenFileNameThatExists_ResultShouldPlayFile()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.PlayMp3File("Start/darthvader_yesmaster.wav").Result;
            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void PlayMp3File_GivenFolder_ResultShouldPlayFile()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.PlayMp3File("Start").Result;
            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void TextToSpeech_GivenModification_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.TextToSpeechEnhanceSpeech("Luke I am your father").Result;
            // assert
            result.Should().NotBeNull();
        }

        [Test]
        public void TextToSpeech_GivenSample_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.TextToSpeech("Luke I am your father").Result;
            // assert
            result.Should().NotBeNull();
        }

        [OneTimeTearDown]
        public void TearDownFixture()
        {
            _disposable.Dispose();
        }

        #region Private Methods

        private static bool EnsureConnected()
        {
            try
            {
                BuildIndicatorApi.Ping().Wait();
                return true;
            }
            catch (Exception e)
            {
                _log.Warn("WebApiIntegrationTests:SetupFixture " + e);
            }
            return false;
        }

        #endregion

//		[Test]
//		public void Enqueue_GivenSoundsThenText_ShouldPlayBoth()
//		{
//			// arrange
//			Setup();
//			// action
//			var choreography = new Choreography();
//			choreography.Sequences.Add(new SequencesPlaySound() { BeginTime = 0 , File = "Start" });
////			choreography.Sequences.Add(new SequencesText2Speech() { BeginTime = 0 , Text = "Mhaha" , DisableTransform = false});
////			choreography.Sequences.Add(new SequencesGpIo() { BeginTime = 0, IsOn = true, PinName = PinName.MainLightGreen});
////			choreography.Sequences.Add(new SequencesInsult() { BeginTime = 0 });
////			choreography.Sequences.Add(new SequencesOneLiner() { BeginTime = 0 });
////			choreography.Sequences.Add(new SequencesQuotes() { BeginTime = 0 });
////			choreography.Sequences.Add(new SequencesTweet() { BeginTime = 0 , Text = "Tweeeet"});
//			var result = BuildIndicatorApi.Enqueue(choreography).Result;
//			// assert
//			result.Should().NotBeNull();
//		}
    }
}