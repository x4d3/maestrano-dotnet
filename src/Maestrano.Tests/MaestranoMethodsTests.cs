using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maestrano.Tests
{
    [TestClass]
    public class MaestranoMethodsTests
    {
        [TestMethod]
        public void Authenticate_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Api.Id = "app-1";
            Maestrano.Api.Key = "somekey";

            Assert.AreEqual(true, Maestrano.Authenticate(Maestrano.Api.Id, Maestrano.Api.Key));
            Assert.AreEqual(false, Maestrano.Authenticate(Maestrano.Api.Id, "someotherrandomkey"));
        }

        [TestMethod]
        public void UnmaskUser_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Assert.AreEqual("usr-1", Maestrano.UnmaskUser("usr-1"));
            Assert.AreEqual("usr-1", Maestrano.UnmaskUser("usr-1.cld-4"));
        }

        [TestMethod]
        public void MaskUser_WhenRealMode_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "real";

            Assert.AreEqual("usr-1", Maestrano.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void MaskUser_WhenVirtualMode_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "virtual";

            Assert.AreEqual("usr-1.cld-1", Maestrano.MaskUser("usr-1", "cld-1"));
        }
    }
}
