using BuildIndicatron.Core.Chat;
using BuildIndicatron.Core.SimpleTextSplit;
using NUnit.Framework;
using FluentAssertions;

namespace BuildIndicatron.Tests.Core.SimpleTextSplit
{
    [TestFixture]
    public class SimpleTextSplitterTests
    {

        #region Setup/Teardown

        public void Setup()
        {
        }

        #endregion
       
        [Test]
        public void Given_GivenSimpleMatch_ShouldReturnValue()
        {
            // arrange
            Setup();
            // action
            var lookup = SimpleTextSplitter.ApplyTo<Result>().Map(@"(ANYTHING)help(ANYTHING)");

            // assert
            var result = lookup.Process("help");
            result.IsMatch.Should().BeTrue();
        }

 
        [Test]
        public void Given_GivenSimpleStringLookup_ShouldReturnValue()
        {
            // arrange
            Setup();
            // action
            var lookup = SimpleTextSplitter.ApplyTo<Result>()
                .Map(@"set setting (?<key>WORD) (?<value>ANYTHING)")
                .Map(@"set setting (?<key>WORD)")
                .Map(@"set setting")
                ;

            // assert
            lookup.Process("help").IsMatch.Should().BeFalse();
            var textSplitterResult = lookup.Process("set setting");
            textSplitterResult.IsMatch.Should().BeTrue();
            lookup.Process("set setting").Value.Key.Should().Be(null);
            lookup.Process("set setting test").Value.Key.Should().Be("test");
            lookup.Process("set setting test fasdf,asdf").Value.Key.Should().Be("test");
            lookup.Process("set setting test fasdf,asdf").Value.Value.Should().Be("fasdf,asdf");
        }


        public class Result
        {
            public bool IsRequestingHelp { get; set; }
            public string Key { get; set; }
            public string Value { get; set; } 
        }
    }

    
}