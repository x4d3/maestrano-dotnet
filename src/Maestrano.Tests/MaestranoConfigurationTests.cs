using NUnit.Framework;
using System;

namespace Maestrano.Tests
{
    [TestFixture]
    public class MaestranoConfigurationTests
    {

        [Test]
        public void itRetrieveTheDevPlatformParameters()
        {
            var config = Configuration.DevPlatform.Load();
            Assert.AreEqual("https://wrong-url.com", config.Host);
            Assert.AreEqual("/and-wrong-path/too", config.ApiPath);
            Assert.AreEqual("[your environment nid]", config.Environment.Name);
            Assert.AreEqual("[your environment key]", config.Environment.ApiKey);
            Assert.AreEqual("[your environment secret]", config.Environment.ApiSecret);
        }

    }
}
