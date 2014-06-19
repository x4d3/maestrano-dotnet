using System;
using Maestrano.Sso;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{
    // Stub class for Saml.Response
    public class ResponseStub : Response
    {
        public ResponseStub()
        {
            _cachedAttributes = new NameValueCollection();
            _cachedAttributes.Add("mno_session", "7ds8f9789a7fd7x0b898bvb8vc9h0gg");
            _cachedAttributes.Add("mno_session_recheck", DateTime.UtcNow.ToString("o"));
            _cachedAttributes.Add("group_uid", "cld-1");
            _cachedAttributes.Add("group_role", "Admin");
            _cachedAttributes.Add("uid", "usr-1");
            _cachedAttributes.Add("virtual_uid", "user-1.cld-1");
            _cachedAttributes.Add("email", "j.doe@doecorp.com");
            _cachedAttributes.Add("virtual_email", "user-1.cld-1@mail.maestrano.com");
            _cachedAttributes.Add("name", "John");
            _cachedAttributes.Add("surname", "Doe");
            _cachedAttributes.Add("country", "AU");
            _cachedAttributes.Add("company_name", "DoeCorp");

        }
    }

    [TestClass]
    public class BaseUserTests
    {
        [TestMethod]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            ResponseStub samlResp = new ResponseStub();
            var att = samlResp.GetAttributes();
            var user = new BaseUser(samlResp);

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
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "real";

            // Build user
            ResponseStub samlResp = new ResponseStub();
            var user = new BaseUser(samlResp);

            Assert.AreEqual(user.Uid, user.ToUid());
        }

        [TestMethod]
        public void ToUid_WhenVirtual_ShouldReturnTheRightUid()
        {
            // Configure environment
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "virtual";

            // Build user
            ResponseStub samlResp = new ResponseStub();
            var user = new BaseUser(samlResp);

            Assert.AreEqual(user.VirtualUid, user.ToUid());
        }

        [TestMethod]
        public void ToEmail_WhenReal_ShouldReturnTheRightEmail()
        {
            // Configure environment
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "real";

            // Build user
            ResponseStub samlResp = new ResponseStub();
            var user = new BaseUser(samlResp);

            Assert.AreEqual(user.Email, user.ToEmail());
        }

        [TestMethod]
        public void ToEmail_WhenVirtual_ShouldReturnTheRightEmail()
        {
            // Configure environment
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "virtual";

            // Build user
            ResponseStub samlResp = new ResponseStub();
            var user = new BaseUser(samlResp);

            Assert.AreEqual(user.VirtualEmail, user.ToEmail());
        }
    }
}
