using System;
using Maestrano.Sso;
using Maestrano.Saml;
using NUnit.Framework;
using System.Collections.Specialized;
using Maestrano.Configuration;

namespace Maestrano.Tests.Sso
{

    [TestFixture]
    public class UserTests
    {
        [Test]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            Preset preset = new Preset("test");
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

    }
}
