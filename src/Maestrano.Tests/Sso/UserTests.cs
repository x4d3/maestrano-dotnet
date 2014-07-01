using System;
using Maestrano.Sso;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{

    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            var samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var user = new User(samlResp);

            Assert.AreEqual(att["mno_session"], user.SsoSession);
            Assert.AreEqual(DateTime.Parse(att["mno_session_recheck"]), user.SsoSessionRecheck);
            Assert.AreEqual(att["group_uid"], user.GroupUid);
            Assert.AreEqual(att["group_role"], user.GroupRole);
            Assert.AreEqual(att["uid"], user.Uid);
            Assert.AreEqual(att["virtual_uid"], user.VirtualUid);
            Assert.AreEqual(att["email"], user.Email);
            Assert.AreEqual(att["virtual_email"], user.VirtualEmail);
            Assert.AreEqual(att["name"], user.FirstName);
            Assert.AreEqual(att["surname"], user.LastName);
            Assert.AreEqual(att["country"], user.Country);
            Assert.AreEqual(att["company_name"], user.CompanyName);
        }

        [TestMethod]
        public void ToUid_WhenReal_ShouldReturnTheRightUid()
        {
            // Configure environment
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "real";

            // Build user
            var samlResp = new SsoResponseStub();
            var user = new User(samlResp);

            Assert.AreEqual(user.Uid, user.ToUid());
        }

        [TestMethod]
        public void ToUid_WhenVirtual_ShouldReturnTheRightUid()
        {
            // Configure environment
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "virtual";

            // Build user
            var samlResp = new SsoResponseStub();
            var user = new User(samlResp);

            Assert.AreEqual(user.VirtualUid, user.ToUid());
        }

        [TestMethod]
        public void ToEmail_WhenReal_ShouldReturnTheRightEmail()
        {
            // Configure environment
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "real";

            // Build user
            var samlResp = new SsoResponseStub();
            var user = new User(samlResp);

            Assert.AreEqual(user.Email, user.ToEmail());
        }

        [TestMethod]
        public void ToEmail_WhenVirtual_ShouldReturnTheRightEmail()
        {
            // Configure environment
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "virtual";

            // Build user
            var samlResp = new SsoResponseStub();
            var user = new User(samlResp);

            Assert.AreEqual(user.VirtualEmail, user.ToEmail());
        }
    }
}
