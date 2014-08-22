using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using BuildIndicatron.Server.Tests.Base;
using BuildIndicatron.Shared.Models.ApiResponses;
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
			try
			{
				BuildIndicatorApi.Ping().Wait();
			}
			catch (Exception e)
			{
				_log.Warn("WebApiIntegrationTests:SetupFixture value");
			}
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

	}
}