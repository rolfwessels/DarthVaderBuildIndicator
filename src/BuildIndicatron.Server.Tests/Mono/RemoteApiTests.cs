using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Server.Tests.Base;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.Composition;
using FluentAssertions;
using log4net;
using NUnit.Framework;
using Renci.SshNet;
using RestSharp;

namespace BuildIndicatron.Server.Tests.Mono
{
    [TestFixture]
    [Explicit]
    //[Timeout(300000)]
    public class RemoteApiTests : BaseIntegrationTests
    {
        private readonly string Host = EnvSettings.Instance.SshHost;
        private readonly string UserName = EnvSettings.Instance.SshUser;
        private readonly string _password = EnvSettings.Instance.SshPassword;
        private readonly string BaseUri = "http://" + EnvSettings.Instance.SshHost + ":8081/";
        private readonly string BaseApiUri = "http://" + EnvSettings.Instance.SshHost + ":8081/api";
        private const string HomePiBuildindicatronServer = "/home/pi/buildIndicatron.server/";
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static SshClient _client;
        private SshCommand _runCommand;
        private StreamReader streamReader;

        #region Setup/Teardown

        [OneTimeTearDown]
        public void FixtureSetup()
        {
            CopyTheLatestSourceFiles();
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
            IRestResponse executeTaskAsync = client.ExecAsyncGet(request).Result;
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
            var result = BuildIndicatorApi.PlayMp3File("Fail").Result;
            // assert
            result.Should().NotBeNull();
        }


        [Test]
        public void TextToSpeech_GivenSample_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
//			var result = BuildIndicatorApi.TextToSpeech("This is some very long text , that just goes on and on. and on. and on. and on. and on. and on. and on. and on. Wow that was boring !").Result;
            var result = BuildIndicatorApi.TextToSpeech("Luke I am your father").Result;
            //assert
            result.Should().NotBeNull();
        }

        [Test]
        public void TextToSpeech_GivenModification_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.TextToSpeechEnhanceSpeech("What are you talking about").Result;
            // assert
            result.Should().NotBeNull();
        }


        [Test]
        public void GpIoOutput_GivenPinId_ShouldPlayAudio()
        {
            // arrange
            Setup();
            // action
            var result = BuildIndicatorApi.GpIoOutput(PinName.SecondaryLightBlue, true).Result;
            // assert
            result.Should().NotBeNull();
        }


        [Test]
        public void Enqueue_GivenSoundsThenText_ShouldPlayBoth()
        {
            // arrange
            Setup();
            // action
            var choreography = new Choreography();
            choreography.Sequences.Add(new SequencesPlaySound() {BeginTime = 50, File = "Start"});

            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightBlue,
                IsOn = true
            });
            choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = 50, Text = "Main Blue"});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightBlue,
                IsOn = false
            });

            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightGreen,
                IsOn = true
            });
            choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = 50, Text = "Main Green"});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightGreen,
                IsOn = false
            });

            choreography.Sequences.Add(
                new SequencesGpIo() {BeginTime = 50, PinName = PinName.MainLightRed, IsOn = true});
            choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = 50, Text = "Main Red"});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightRed,
                IsOn = false
            });

            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.SecondaryLightGreen,
                IsOn = true
            });
            choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = 50, Text = "Secondary Green"});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.SecondaryLightGreen,
                IsOn = false
            });

            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.SecondaryLightRed,
                IsOn = true
            });
            choreography.Sequences.Add(new SequencesText2Speech() {BeginTime = 50, Text = "Secondary Red"});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.SecondaryLightRed,
                IsOn = false
            });

            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightBlue,
                IsOn = true
            });
            choreography.Sequences.Add(new SequencesInsult() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesOneLiner() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesQuotes() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 50,
                PinName = PinName.MainLightBlue,
                IsOn = false
            });
//			choreography.Sequences.Add(new SequencesTweet() { BeginTime = 0, Text = "Tweeeet" });
            var result = BuildIndicatorApi.Enqueue(choreography).Result;
            // assert
            result.Should().NotBeNull();
            var size = result.QueueSize;
            size.Should().Be(choreography.Sequences.Count);
            while (size > 0)
            {
                var queueSize = BuildIndicatorApi.GetQueueSize();
                size = queueSize.Result.QueueSize;
                Thread.Sleep(1000);
            }
        }

        [OneTimeSetUp]
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
            _client = new SshClient(Host, UserName, _password);
            _client.Connect();
            _client.RunCommand("sudo pkill mono");
            var call = string.Format("cd {0}", HomePiBuildindicatronServer);
            const string commandText = "sudo mono BuildIndicatron.Server.exe";
            var text = call + " && " + commandText;
            _log.Info("Starting command:" + text);
            _runCommand = _client.CreateCommand(text);
            _runCommand.BeginExecute();
            streamReader = new StreamReader(_runCommand.OutputStream);
            WaitFor("Running", 6000).Wait();
        }

        private void CopyTheLatestSourceFiles()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            IEnumerable<string> directoryInfo =
                Directory.GetFiles(currentDirectory, "BuildIndicatron*.*", SearchOption.AllDirectories)
                    .Union(Directory.GetFiles(currentDirectory, "*.html", SearchOption.AllDirectories));
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(Host, UserName, _password);

            var scpClient = new ScpClient(connectionInfo);
            scpClient.Connect();
            _log.Info(string.Format("Copy {0} files", directoryInfo.Count()));
            foreach (string source in directoryInfo.Where(x => !x.Contains(".Test.dll")))
            {
                string sourceName = source.Replace(currentDirectory + "\\", "");
                string remoteFileName = Path.Combine(HomePiBuildindicatronServer, sourceName.Replace("\\", "/"));
                _log.Info(string.Format("Upload:{0} to {1}", sourceName, remoteFileName));
                scpClient.Upload(new FileInfo(source), remoteFileName);
            }
            scpClient.Disconnect();
        }


        private void EndService()
        {
            _client.Disconnect();
            Console.Out.WriteLine("Disconnect");
        }

        #endregion
    }
}