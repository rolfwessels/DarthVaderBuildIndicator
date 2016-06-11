using System.IO;
using System.Reflection;
using BuildIndicatron.Core.Helpers;
using BuildIndicatron.Core.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace BuildIndicatron.Tests.Core.Settings
{
    [TestFixture]
    public class SettingsManagerTests
    {
        private SettingsManager _securityManager;
        private string _tmpFileName;

        #region Setup/Teardown

        public void Setup()
        {
            _tmpFileName = Path.GetTempFileName();
            _securityManager = new SettingsManager(_tmpFileName);
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_tmpFileName);
        }

        #endregion

        [Test]
        public void GetSetting_GivenDefaultValue_ShouldReturnDefault()
        {
            // arrange
            Setup();
            // action
            string setting = _securityManager.Get("test", "Mother");
            // assert
            setting.Should().Be("Mother");
        }

        [Test]
        public void GetSetting_GivenLoadingByDefault_ShouldReturnDefault()
        {
            // arrange
            Setup();
            // action
            string setting = _securityManager.Get("test");
            // assert
            setting.Should().Be(null);
        }

        [Test]
        public void SetSetting_GiventestingFor_ShouldSave()
        {
            // arrange
            Setup();
            // action
            _securityManager.Set("test", "tests");
            // assert
            _securityManager.Get("test").Should().Be("tests");
        }

        [Test]
        public void SetSetting_GivenDuplicateKey_ShouldSaveSecondOne()
        {
            // arrange
            Setup();
            // action
            _securityManager.Set("test", "tests1");
            _securityManager.Set("test", "tests2");
            // assert
            _securityManager.Get("test").Should().Be("tests2");
        }

        [Test]
        public void SetSetting_GivenValue_ShouldPersistSetting()
        {
            // arrange
            Setup();
            // action
            _securityManager.Set("test1", "tests1");
            _securityManager.Set("test2", "tests2");
            // assert
            var fileInfo = new FileInfo(_tmpFileName);
            fileInfo.Exists.Should().BeTrue();
            fileInfo.Length.Should().BeGreaterThan(5);
        }

        [Test]
        public void GetSetting_GivenSaved_ShouldLoadPersistedValue()
        {
            
            // arrange
            Setup();
            _securityManager.Set("test1", "tests1");
            var loading = new SettingsManager(_tmpFileName);
            // action
            var setting = loading.Get("test1");
            // assert
            setting.Should().Be("tests1");
        }
    }
}