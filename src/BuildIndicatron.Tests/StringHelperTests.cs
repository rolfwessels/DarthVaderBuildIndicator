using System.Security.Cryptography;
using System.Text;
using BuildIndicatron.Core.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests
{
    [TestFixture]
    public class StringHelperTests

    {
        [Test]
        public void MD5Hash_GivenText_ShouldAlwaysReturnSameResult()
        {
            // arrange
            var md5Hash = StringHelper.Md5Hash("SampleName");
            // assert
            md5Hash.Should().Be("ngYGjMjicFY4phqPeYXPrg");
        }

        [Test]
        public void MaskInput_GivenEmptyString_ShouldReturnNull()
        {
            // arrange
            var result = StringHelper.MaskInput("");
            // assert
            result.Should().Be(null);
        }

        [Test]
        public void MaskInput_GivenNull_ShouldReturnNull()
        {
            // arrange
            var result = StringHelper.MaskInput(null);
            // assert
            result.Should().Be(null);
        }


        [Test]
        public void MaskInput_GivenValue_ShouldMaskedValue()
        {
            // arrange
            var result = StringHelper.MaskInput("b5d985333f07354a634deb9d2b37f");
            // assert
            result.Should().Be(null);
        }
    }
}