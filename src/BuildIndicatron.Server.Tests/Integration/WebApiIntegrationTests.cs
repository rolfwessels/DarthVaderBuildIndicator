using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using BuildIndicatron.Server.Tests.Base;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using FizzWare.NBuilder.Generators;
using FluentAssertions;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using RestSharp;
using log4net;
using System.Linq;

namespace BuildIndicatron.Server.Tests.Integration
{
	[TestFixture]
	public class WebApiIntegrationTests : BaseIntegrationTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private IDisposable _disposable;

		#region Setup/Teardown
		[TestFixtureSetUp]
		public void SetupFixture()
		{
			var options = new StartOptions
			{
				ServerFactory = "Nowin",
				Port = GetRandom.Int(19000,19999)
			};
			var baseUri = string.Format("http://localhost:{0}/api", options.Port);
			_log.Info(string.Format("Starting api on {0}", baseUri));
			_disposable = WebApp.Start<Startup>(options);
			_log.Info("Started");
			
			CreateClient(baseUri);


            this.WaitFor((t) => EnsureConnected(), 10000);
		}

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

	    public void Setup()
		{

		}

		[TearDown]
		public void TearDown()
		{

		}

		[TestFixtureTearDown]
		public void TearDownFixture()
		{
			_disposable.Dispose();
		}

		#endregion

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
		public void TextToSpeech_GivenSample_ShouldPlayAudio()
		{
			// arrange
			Setup();
			// action
			var result = BuildIndicatorApi.TextToSpeech("Luke I am your father").Result;
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
		public void GpIoOutput_GivenPinId_ShouldPlayAudio()
		{
			// arrange
			Setup();
			// action
			var result = BuildIndicatorApi.GpIoOutput(12,true).Result;
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