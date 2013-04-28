using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Tests.IntegrationTests
{
    [TestFixture]
    public class LocalIntegrationTests
    {
        private IRobotApi _robotApi;
        protected string _hostApi;

        public LocalIntegrationTests()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("Log4Net.config"));
            //_hostApi = Config.Url;
            _hostApi = "http://192.168.1.13:5000/";
        }

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            
            _robotApi = new RobotApi(_hostApi);
        }

        [TearDown]
        public void TearDown()
        {

        }

        #endregion

        [Test]
        public async Task Ping()
        {
            var result = await _robotApi.Ping();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task PlayMp3File_Call_ValidResponse()
        {
            var result = await _robotApi.PlayMp3File("Funny");
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task PlayMp3File_CallOther_ValidResponse()
        {
            var result = await _robotApi.PlayMp3File("Start");
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task TextToSpeech_Call_ValidResponse()
        {
            var result = await _robotApi.TextToSpeech("Nice");
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task SetupGpIo_Call_ValidResponse()
        {
            var result = await _robotApi.GpIoSetup(25,Gpio.Out);
            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task GpIoOutput_Call_ValidResponse()
        {
            var result = await _robotApi.GpIoOutput(24, false);
            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task GpIoOutput_Call_ValidResponse1()
        {
            var result = await _robotApi.GpIoOutput(17, true);
            result.Should().NotBeNull();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task PassiveProcess_Call_ValidResponse()
        {
            var result = await _robotApi.PassiveProcess();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task PassiveProcess_CallToSetThePassive_ValidResponse()
        {
            var result = await _robotApi.PassiveProcess(new Passive()
            {
                Interval = 5,
                StartTime = 7,
                SleepTime = 20,
                Compositions = new List<Choreography>()
                {
                    new Choreography() {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesPlaySound() {File = "Funny"},
                        }   
                    } , 
                    new Choreography() {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesPlaySound() {File = "Wtf"},
                        }   
                    },
                    new Choreography() {
                    Sequences = new List<Sequences>()
                        {
                            new SequencesOneLiner(),
                        }   
                    }

                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task Enqueue_Call_ValidResponse()
        {
            var pinRed = 17;
            var pinGreen = 24;
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesGpIo() { BeginTime=0, Pin=pinRed , IsOn = true },
                    new SequencesGpIo() { BeginTime=1000, Pin=pinRed , IsOn = false },
                    new SequencesGpIo() { BeginTime=1000, Pin=pinGreen , IsOn = true },
                    new SequencesGpIo() { BeginTime=2000, Pin=pinGreen , IsOn = false }
                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

         [Test]
        public async Task Werner_Show_oFf()
        {
             
            var pinRed = 27;
            var pinBlue = 11;
            var pinGreen = 9;
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesGpIo() { BeginTime=0, Pin=pinRed , IsOn = false },
                    new SequencesGpIo() { BeginTime=0, Pin=pinBlue , IsOn = true },
                    new SequencesGpIo() { BeginTime=0, Pin=pinGreen , IsOn = false },

                    
                    new SequencesGpIo() { BeginTime=2999, Pin=pinBlue , IsOn = false },
                    new SequencesGpIo() { BeginTime=2999, Pin=pinRed , IsOn = true },
                    new SequencesPlaySound() {BeginTime=3000, File = "Fail"},
                    
                    
                    new SequencesGpIo() { BeginTime=13000, Pin=pinBlue , IsOn = true },
                    new SequencesPlaySound() {BeginTime=14000, File = "Success"},
                    new SequencesGpIo() { BeginTime=14000, Pin=pinGreen , IsOn = true },
                    new SequencesGpIo() { BeginTime=14000, Pin=pinBlue , IsOn = false },
                    new SequencesGpIo() { BeginTime=14000, Pin=pinRed , IsOn = false },
                    new SequencesText2Speech() { BeginTime=14000, Text = "Wynand !!! How cool is this!" }
                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }


        
        [Test]
        public async Task Enqueue_Call2_ValidResponse()
        {
            var pinBlue = 11;
            var pinGreen = 9;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesGpIo() { BeginTime=0, Pin=pinBlue , IsOn = true },
                    new SequencesPlaySound() {File = "Success"},
                    new SequencesGpIo() { BeginTime=4000, Pin=pinBlue , IsOn = false },
                    new SequencesGpIo() { BeginTime=4000, Pin=pinGreen , IsOn = true },
                    new SequencesText2Speech() { BeginTime=5000, Text = "Wicked this seems to work" }
                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task Enqueue_Call3_ValidResponse()
        {
            var pinRed = 17;
            var pinGreen = 24;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesGpIo() { BeginTime=0, Pin=pinGreen , IsOn = true },
                    new SequencesText2Speech() { BeginTime=0, Text = "Build Done by rolf has passed" },
                    new SequencesGpIo() { BeginTime=0, Pin=pinGreen , IsOn = false },
                    new SequencesGpIo() { BeginTime=0, Pin=pinRed , IsOn = true },
                    new SequencesPlaySound() {File = "Fail"},
                    new SequencesGpIo() { BeginTime=4000, Pin=pinRed , IsOn = true },
                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task SetButtonChoreography_Call_ValidResponse()
        {
            var result = await _robotApi.SetButtonChoreography(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesText2Speech() { BeginTime=0, Text = "20 builds passed. 1 failed. development bla bla bla" }
                }
            });


            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task SetButtonChoreography_CallWithMultipleChoreography_ValidResponse()
        {
            var result = await _robotApi.SetButtonChoreography(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesText2Speech() { BeginTime=0, Text = "20 builds passed. 1 failed. development bla bla bla" }
                }
            },
            new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesPlaySound() { BeginTime=0, File = "Funny"}
                }
            }
            );


            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task GetClips_Call_ValidResponse()
        {
            var result = await _robotApi.GetClips();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.Folders.Count.Should().BeGreaterOrEqualTo(4);
            result.Folders.First().Files.Count.Should().BeGreaterOrEqualTo(2);
        }

        [Test]
        public async Task Enqueue_OneLiner()
        {
            var pinGreen = 24;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() { 
                    new SequencesGpIo() { BeginTime=0, Pin=pinGreen , IsOn = true },
                    new SequencesOneLiner()
                }
            });
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }
    }

}