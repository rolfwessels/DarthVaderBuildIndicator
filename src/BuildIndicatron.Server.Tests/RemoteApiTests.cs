using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using NUnit.Framework;
using FluentAssertions;
using Renci.SshNet;
using RestSharp;
using log4net;

namespace BuildIndicatron.Server.Tests
{
	[TestFixture]
	public class RemoteApiTests
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private SshCommand _runCommand;
		private IAsyncResult _beginExecute;
		private SshClient _client;
		private StreamReader streamReader;
		private const string Host = "192.168.1.242";
		private const string UserName = "xxxxxxxx";
		private const string Password = "xxxxxxxx";
		private const string _homePiBuildindicatronServer = "/home/pi/buildIndicatron.server/";
		private const string BaseUri = "http://" + Host + ":8080/";
		#region Setup/Teardown
		
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			//CopyTheLatestSourceFiles();
			BeginService();
		}


		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			EndService();
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
		public void Constructor_WhenCalled_ShouldNotBeNull()
		{
			// arrange
			Setup();
			// assert
			var client = new RestClient(BaseUri);
			
			var request = new RestRequest("", Method.GET);
			_log.Info("Placing request to " + client.BuildUri(request));
			var executeTaskAsync = client.Execute(request);
			Console.Out.WriteLine("executeTaskAsync.Content:"+executeTaskAsync.Content);
			executeTaskAsync.Content.Should().Contain("Hello World!");
			
		}

		private Task WaitFor(string yoma, int fromSeconds = 5000)
		{
			return Task.Run(() =>
				{
					var line = "";
					var dateTime = DateTime.Now;
					var fromMilliseconds = TimeSpan.FromMilliseconds(fromSeconds);
					while (line == null || !line.Contains(yoma) && (DateTime.Now - dateTime < fromMilliseconds))
					{
						line = streamReader.ReadLine();
						if (line != null) _log.Info("line:" + line);
					}
				});
		}

		#region Private Methods
		private void BeginService()
		{
			_client = new SshClient(Host, UserName, Password);
			_client.Connect();
			var commandText = string.Format("mono {0}", Path.Combine(_homePiBuildindicatronServer, "BuildIndicatron.Server.exe"));
			_log.Info("Starting command:" + commandText);
			_runCommand = _client.CreateCommand(commandText);
			_beginExecute = _runCommand.BeginExecute();
			streamReader = new StreamReader(_runCommand.OutputStream);
			WaitFor("Running", 6000).Wait();
		}

		private static void CopyTheLatestSourceFiles()
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			var directoryInfo =
				Directory.GetFiles(currentDirectory, "BuildIndicatron*.*", SearchOption.AllDirectories)
						 .Union(Directory.GetFiles(currentDirectory, "*.html", SearchOption.AllDirectories));
			ConnectionInfo connectionInfo = new PasswordConnectionInfo(Host, UserName, Password);
			
			var scpClient = new ScpClient(connectionInfo);
			scpClient.Connect();
			_log.Info(string.Format("Copy {0} files", directoryInfo.Count()));
			foreach (var source in directoryInfo.Where(x => !x.Contains(".Test.dll")))
			{
				var sourceName = source.Replace(currentDirectory + "\\", "");
				var remoteFileName = Path.Combine(_homePiBuildindicatronServer, sourceName.Replace("\\", "/"));
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