using System.Threading.Tasks;
using BuildIndicatron.Core.Processes;
using BuildIndicatron.Server.Core.WebApi.Controllers;
using BuildIndicatron.Shared.Enums;
using BuildIndicatron.Shared.Models.Composition;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using Newtonsoft.Json;

namespace BuildIndicatron.Server.Tests.Controller
{
    [TestFixture]
    public class EnqueueControllerTests
    {
        private EnqueueController _enqueueController;
        private Mock<IStage> _mockIStage;

        #region Setup/Teardown

        public void Setup()
        {
            _mockIStage = new Mock<IStage>(MockBehavior.Strict);
            _enqueueController = new EnqueueController(_mockIStage.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockIStage.VerifyAll();
        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _enqueueController.Should().NotBeNull();
        }

        [Test]
        public void Post_GivenMultipleItems_ShouldAddAllToStage()
        {
            // arrange
            Setup();
            _mockIStage.Setup(mc => mc.Enqueue(It.IsAny<object>()));
            _mockIStage.Setup(mc => mc.Count).Returns(7);
            _mockIStage.Setup(mc => mc.Play()).Returns(Task.FromResult(true));
            // action
            var choreography = new Choreography();
            choreography.Sequences.Add(new SequencesPlaySound() {BeginTime = 0, File = "Startup"});
            choreography.Sequences.Add(new SequencesText2Speech()
            {
                BeginTime = 0,
                Text = "Mhaha",
                DisableTransform = false
            });
            choreography.Sequences.Add(new SequencesGpIo()
            {
                BeginTime = 0,
                IsOn = true,
                PinName = PinName.MainLightGreen
            });
            choreography.Sequences.Add(new SequencesInsult() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesOneLiner() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesQuotes() {BeginTime = 0});
            choreography.Sequences.Add(new SequencesTweet() {BeginTime = 0, Text = "Tweeeet"});
            var serializeObject = JsonConvert.SerializeObject(choreography);
            var deserializeObject = JsonConvert.DeserializeObject<EnqueueController.ChoreographyModel>(serializeObject);
            var result = _enqueueController.Post(deserializeObject);
            // assert
            result.Should().NotBeNull();
            _mockIStage.Verify(mc => mc.Enqueue(It.IsAny<object>()), Times.Exactly(7));
        }
    }
}