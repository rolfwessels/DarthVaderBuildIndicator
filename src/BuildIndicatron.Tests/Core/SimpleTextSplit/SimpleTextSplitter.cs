using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.SimpleTextSplit;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Tests.Core.SimpleTextSplit
{
    [TestFixture]
    public class SimpleTextSplitterTests
    {

        private SimpleTextSplitter _simpleTextSplitter;

        #region Setup/Teardown

        public void Setup()
        {
            _simpleTextSplitter = new SimpleTextSplitter();
        }

        #endregion

        [Test]
        public void Constructor_WhenCalled_ShouldNotBeNull()
        {
            // arrange
            Setup();
            // assert
            _simpleTextSplitter.Should().NotBeNull();
        }

        [Test]
        public void Given_GivenSimpleMatch_ShouldReturnValue()
        {
            // arrange
            Setup();
            // action
            _simpleTextSplitter.Lookup("");
            // assert
        }


    }

}