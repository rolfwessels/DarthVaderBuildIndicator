using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildIndicatron.Core;
using BuildIndicatron.Core.Api;
using BuildIndicatron.Shared.Models.ApiResponses;
using BuildIndicatron.Shared.Models.Composition;
using BuildIndicatron.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.IntegrationTests
{
    [TestFixture]
    [Explicit]
    public class BuildIndicationApiTest
    {
        private IRobotApi _robotApi;
        protected string _hostApi;

        public BuildIndicationApiTest()
        {
            
            _hostApi = Config.Url;
            //_hostApi = "http://192.168.1.242:5000/";
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
        }

        [Test]
        public async Task PlayMp3File_Call_ValidResponse()
        {
            var result = await _robotApi.PlayMp3File("Funny");
            result.Should().NotBeNull();
        }

        [Test]
        public async Task PlayMp3File_CallOther_ValidResponse()
        {
            var result = await _robotApi.PlayMp3File("Start");
            result.Should().NotBeNull();
        }

        [Test]
        public async Task TextToSpeech_Call_ValidResponse()
        {
            var result = await _robotApi.TextToSpeech("Well that is not bad");
            result.Should().NotBeNull();
        }

        [Test]
        public async Task SetupGpIo_Call_ValidResponse()
        {
            var result = await _robotApi.GpIoSetup(25, GpioDirection.Out);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task GpIoOutput_Call_ValidResponse()
        {
            var result = await _robotApi.GpIoOutput(24, false);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task GpIoOutput_Call_ValidResponse1()
        {
            var result = await _robotApi.GpIoOutput(17, true);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task PassiveProcess_Call_ValidResponse()
        {
            var result = await _robotApi.PassiveProcess();
            result.Should().NotBeNull();
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
                    new Choreography()
                    {
                        Sequences = new List<Sequences>()
                        {
                            new SequencesPlaySound() {File = "Funny"},
                        }
                    },
                    new Choreography()
                    {
                        Sequences = new List<Sequences>()
                        {
                            new SequencesPlaySound() {File = "Wtf"},
                        }
                    },
                    new Choreography()
                    {
                        Sequences = new List<Sequences>()
                        {
                            new SequencesOneLiner(),
                        }
                    },
                    new Choreography()
                    {
                        Sequences = new List<Sequences>()
                        {
                            new SequencesQuotes(),
                        }
                    }
                }
            });
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Enqueue_Call_ValidResponse()
        {
            var pinRed = 17;
            var pinGreen = 24;
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesGpIo() {BeginTime = 0, Pin = pinRed, IsOn = true},
                    new SequencesGpIo() {BeginTime = 1000, Pin = pinRed, IsOn = false},
                    new SequencesGpIo() {BeginTime = 1000, Pin = pinGreen, IsOn = true},
                    new SequencesGpIo() {BeginTime = 2000, Pin = pinGreen, IsOn = false}
                }
            });
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Werner_Show_oFf()
        {
            var pinRed = 27;
            var pinBlue = 11;
            var pinGreen = 9;
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesGpIo() {BeginTime = 0, Pin = pinRed, IsOn = false},
                    new SequencesGpIo() {BeginTime = 0, Pin = pinBlue, IsOn = true},
                    new SequencesGpIo() {BeginTime = 0, Pin = pinGreen, IsOn = false},


                    new SequencesGpIo() {BeginTime = 2999, Pin = pinBlue, IsOn = false},
                    new SequencesGpIo() {BeginTime = 2999, Pin = pinRed, IsOn = true},
                    new SequencesPlaySound() {BeginTime = 3000, File = "Fail"},


                    new SequencesGpIo() {BeginTime = 13000, Pin = pinBlue, IsOn = true},
                    new SequencesPlaySound() {BeginTime = 14000, File = "Success"},
                    new SequencesGpIo() {BeginTime = 14000, Pin = pinGreen, IsOn = true},
                    new SequencesGpIo() {BeginTime = 14000, Pin = pinBlue, IsOn = false},
                    new SequencesGpIo() {BeginTime = 14000, Pin = pinRed, IsOn = false},
                    new SequencesText2Speech() {BeginTime = 14000, Text = "Wynand !!! How cool is this!"}
                }
            });
            result.Should().NotBeNull();
        }


        [Test]
        public async Task Enqueue_Call2_ValidResponse()
        {
            var pinBlue = 11;
            var pinGreen = 9;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesGpIo() {BeginTime = 0, Pin = pinBlue, IsOn = true},
                    new SequencesPlaySound() {File = "Success"},
                    new SequencesGpIo() {BeginTime = 4000, Pin = pinBlue, IsOn = false},
                    new SequencesGpIo() {BeginTime = 4000, Pin = pinGreen, IsOn = true},
                    new SequencesText2Speech() {BeginTime = 5000, Text = "Wicked this seems to work"}
                }
            });
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Enqueue_Call3_ValidResponse()
        {
            var pinRed = 17;
            var pinGreen = 24;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesGpIo() {BeginTime = 0, Pin = pinGreen, IsOn = true},
                    new SequencesText2Speech() {BeginTime = 0, Text = "Build Done by rolf has passed"},
                    new SequencesGpIo() {BeginTime = 0, Pin = pinGreen, IsOn = false},
                    new SequencesGpIo() {BeginTime = 0, Pin = pinRed, IsOn = true},
                    new SequencesPlaySound() {File = "Fail"},
                    new SequencesGpIo() {BeginTime = 4000, Pin = pinRed, IsOn = true},
                }
            });
            result.Should().NotBeNull();
        }

        [Test]
        public async Task SetButtonChoreography_Call_ValidResponse()
        {
            var result = await _robotApi.SetButtonChoreography(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesText2Speech()
                    {
                        BeginTime = 0,
                        Text = "20 builds passed. 1 failed. development bla bla bla"
                    }
                }
            });


            result.Should().NotBeNull();
        }

        [Test]
        public async Task SetButtonChoreography_CallWithMultipleChoreography_ValidResponse()
        {
            var result = await _robotApi.SetButtonChoreography(new Choreography()
                {
                    Sequences = new List<Sequences>()
                    {
                        new SequencesText2Speech()
                        {
                            BeginTime = 0,
                            Text = "20 builds passed. 1 failed. development bla bla bla"
                        }
                    }
                },
                new Choreography()
                {
                    Sequences = new List<Sequences>()
                    {
                        new SequencesPlaySound() {BeginTime = 0, File = "Funny"}
                    }
                });


            result.Should().NotBeNull();
        }

        [Test]
        public async Task TweetSomething()
        {
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() {new SequencesTweet() {BeginTime = 0, Text = _insults.Random()}}
            });

            result.Should().NotBeNull();
        }

        [Test]
        public async Task QuoteSomething()
        {
            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>() {new SequencesQuotes() {BeginTime = 0}}
            });

            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetClips_Call_ValidResponse()
        {
            var result = await _robotApi.GetClips();
            result.Should().NotBeNull();


            result.Folders.Count.Should().BeGreaterOrEqualTo(4);
            result.Folders.First().Files.Count.Should().BeGreaterOrEqualTo(2);
        }


        [Test]
        public async Task Enqueue_OneLiner()
        {
            var pinGreen = 24;

            var result = await _robotApi.Enqueue(new Choreography()
            {
                Sequences = new List<Sequences>()
                {
                    new SequencesGpIo() {BeginTime = 0, Pin = pinGreen, IsOn = true},
                    new SequencesOneLiner()
                }
            });
            result.Should().NotBeNull();
        }


        private readonly string[] _insults = new[]
        {
            "Your birth certificate is an apology letter from the condom factory.",
            "You must have been born on a highway because that's where most accidents happen.",
            "I don't exactly hate you, but if you were on fire and I had water, I'd drink it.",
            "You are proof that God has a sense of humor.",
            "Shut up, you'll never be the man your mother is.",
            "Hey, you have somthing on your chin... no, the 3rd one down",
            "Do you wanna lose ten pounds of ugly fat? Cut off your head",
            "Am I getting smart with you? How would you know?",
            "Your family tree must be a cactus because everybody on it is a prick.",
            "I'd like to see things from your point of view but I can't seem to get my head that far up my ass.",
            "It's better to let someone think you are an Idiot than to open your mouth and prove it.",
            "You are so stupid, you'd trip over a cordless phone.",
            "You are so old, your birth-certificate expired.",
            "Well I could agree with you, but then we'd both be wrong.",
            "So, a thought crossed your mind? Must have been a long and lonely journey.",
            "Why don't you slip into something more comfortable -- like a coma.",
            "If assholes could fly, this place would be an airport!",
            "Are you always an idiot, or just when I'm around?",
            "You stare at frozen juice cans because they say, concentrate.",
            "Come again when you can't stay quite so long.",
            "You may not be the best looking girl here, but beauty is only a light switch away!",
            "It looks like your face caught on fire and someone tried to put it out with a hammer.",
            "I love what you've done with your hair. How do you get it to come out of the nostrils like that?",
            "You do realize makeup isn't going to fix your stupidity?",
            "Ordinarily people live and learn. You just live.",
            "Looks like you traded in your neck for an extra chin!",
            "Don't you need a license to be that ugly?",
            "You occasionally stumble over the truth, but you quickly pick yourself up and carry on as if nothing happened.",
            "I've seen people like you, but I had to pay admission!",
            "Jesus loves you, everyone else thinks you're an asshole!",
            "Shock me, say something intelligent.",
            "Don't feel sad, don't feel blue, Frankenstein was ugly too.",
            "Aww, it's so cute when you try to talk about things you don't understand.",
            "You are proof that evolution CAN go in reverse.",
            "You are so old, you fart dust.",
            "I may be fat, but you're ugly, and I can lose weight.",
            "I heard you took an IQ test and they said you're results were negative.",
            "If I want your opinion, I'll give it to you.",
            "I wish you no harm, but it would have been much better if you had never lived.",
            "When was the last time you could see your whole body in the mirror?",
            "If a crackhead saw you, he'd think he needs to go on a diet.",
            "Being around you is like having a cancer of the soul.",
            "Even if you were twice as smart, you'd still be stupid!",
            "We all sprang from apes, but you didn't spring far enough.",
            "Learn from your parents' mistakes - use birth control!",
            "If what you don't know can't hurt you, you're invulnerable.",
            "Do you still love nature, despite what it did to you?",
            "Your parents hated you so much you bath toys were an iron and a toaster",
            "You're as useless as a screen door on a submarine.",
            "Maybe if you ate some of that makeup you could be pretty on the inside.",
            "I'd like to help you out. Which way did you come in?",
            "If you had another brain, it would be lonely.",
            "I don't know what makes you so stupid, but it really works!",
            "You must think you're strong, but you only smell strong.",
            "The best part of you is still running down your old mans leg.",
            "100,000 sperm, you were the fastest?",
            "Yeah you're pretty, pretty stupid",
            "Nice tan, orange is my favorite color.",
            "Is your name Maple Syrup? It should be, you sap.",
            "Brains aren't everything. In your case they're nothing.",
            "I look into your eyes and get the feeling someone else is driving.",
            "If you spoke your mind, you'd be speechless.",
            "Ever since I saw you in your family tree, I've wanted to cut it down.",
            "Just reminding u there is a very fine line between hobby and mental illness.",
            "Your mom must have a really loud bark!",
            "Are your parents siblings?",
            "You're as useful as an ashtray on a motorcycle.",
            "You act like your arrogance is a virtue.",
            "Beauty is skin deep, but ugly is to the bone.",
            "You're the reason why women earn 75 cents to the dollar.",
            "When anorexics see you, they think they need to go on a diet.",
            "I hear the only place you're ever invited is outside.",
            "Please tell me you don't home-school your kids.",
            "You'll make a great first wife some day.",
            "You are so old, even your memory is in black and white.",
            "For those who never forget a face, you are an exception.",
            "People like you are the reason I work out.",
            "You're stupid because you're blonde."
        };
    }
}