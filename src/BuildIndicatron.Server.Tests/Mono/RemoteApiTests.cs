using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuildIndicatron.Server.Tests.Base;
using FluentAssertions;
using NUnit.Framework;
using Renci.SshNet;
using RestSharp;
using log4net;

namespace BuildIndicatron.Server.Tests
{
	[TestFixture]
	[Explicit]
	public class RemoteApiTests : BaseIntegrationTests
	{
		private const string Host = "192.168.1.242";
		private const string UserName = "pi";
		private const string Password = "***REMOVED***";
		private const string _homePiBuildindicatronServer = "/home/pi/buildIndicatron.server/";
		private const string BaseUri = "http://" + Host + ":8080/";
		private const string BaseApiUri = BaseUri+"api";
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private IAsyncResult _beginExecute;
		private static SshClient _client;
		private SshCommand _runCommand;
		private StreamReader streamReader;

		#region Setup/Teardown

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			//CopyTheLatestSourceFiles();
			BeginService();
			CreateClient(BaseApiUri);
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
		
		public void HomePage_WhenCalled_ShouldContainHelloWorld()
		{
			// arrange
			Setup();
			// assert
			var client = new RestClient(BaseUri);
			var request = new RestRequest("", Method.GET);
			_log.Info("Placing request to " + client.BuildUri(request));
			IRestResponse executeTaskAsync = client.Execute(request);
			Console.Out.WriteLine("executeTaskAsync.Content:" + executeTaskAsync.Content);
			executeTaskAsync.Content.Should().Contain("Hello World!");
		}

		[Test]
	
		public void Ping_WhenCalled_ShouldContainHelloWorld()
		{
			// arrange
			Setup();
			// assert
			var ping = BuildIndicatorApi.Ping().Result;
			// assert
			ping.Version.Should().NotBeEmpty();
			ping.Platform.Should().NotBeEmpty();
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

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			EndService();
		}

		#region Private Methods

		private Task WaitFor(string yoma, int milliseconds = 5000)
		{
			return Task.Run(() =>
				{
					string line = "";
					DateTime timeOut = DateTime.Now.Add(TimeSpan.FromMilliseconds(milliseconds));
					while (line == null || !line.Contains(yoma) && (DateTime.Now < timeOut))
					{
						line = streamReader.ReadLine();
						if (line != null) _log.Info("line:" + line);
					}
				});
		}

		private void BeginService()
		{
			_client = new SshClient(Host, UserName, Password);
			_client.Connect();
			var call = string.Format("cd {0}", _homePiBuildindicatronServer);
			const string commandText = "mono BuildIndicatron.Server.exe";
			var text = call + " && " + commandText;
			_log.Info("Starting command:" + text);
			_runCommand = _client.CreateCommand(text);
			_beginExecute = _runCommand.BeginExecute();
			streamReader = new StreamReader(_runCommand.OutputStream);
			WaitFor("Running", 6000).Wait();
		}

		private static void CopyTheLatestSourceFiles()
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			IEnumerable<string> directoryInfo =
				Directory.GetFiles(currentDirectory, "BuildIndicatron*.*", SearchOption.AllDirectories)
				         .Union(Directory.GetFiles(currentDirectory, "*.html", SearchOption.AllDirectories));
			ConnectionInfo connectionInfo = new PasswordConnectionInfo(Host, UserName, Password);

			var scpClient = new ScpClient(connectionInfo);
			scpClient.Connect();
			_log.Info(string.Format("Copy {0} files", directoryInfo.Count()));
			foreach (string source in directoryInfo.Where(x => !x.Contains(".Test.dll")))
			{
				string sourceName = source.Replace(currentDirectory + "\\", "");
				string remoteFileName = Path.Combine(_homePiBuildindicatronServer, sourceName.Replace("\\", "/"));
				_log.Info(string.Format("Upload:{0} to {1}", sourceName, remoteFileName));
				scpClient.Upload(new FileInfo(source), remoteFileName);
			}
			scpClient.Disconnect();
		}


		private void EndService()
		{
			_client.RunCommand("pkill mono");
			_client.Disconnect();
			Console.Out.WriteLine("Disconnect");
		}

		#endregion
	}
}