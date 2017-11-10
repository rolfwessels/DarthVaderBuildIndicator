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
    }
}