using System;
using Maestrano.Sso;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{
    // Stub class for Saml.Response
    public class SsoGroupResponseStub : Response
    {
        public SsoGroupResponseStub()
        {
            _cachedAttributes = new NameValueCollection();
            _cachedAttributes.Add("mno_session", "7ds8f9789a7fd7x0b898bvb8vc9h0gg");
            _cachedAttributes.Add("mno_session_recheck", DateTime.UtcNow.ToString("o"));
            _cachedAttributes.Add("group_uid", "cld-1");
            _cachedAttributes.Add("group_role", "Admin");
            _cachedAttributes.Add("group_end_free_trial", DateTime.UtcNow.ToString("o"));
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
    public class GroupTests
    {
        [TestMethod]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            SsoGroupResponseStub samlResp = new SsoGroupResponseStub();
            var att = samlResp.GetAttributes();
            var group = new Group(samlResp);

            Assert.AreEqual(att["group_uid"], group.Uid);
            Assert.AreEqual(DateTime.Parse(att["group_end_free_trial"]), group.FreeTrialEndAt);
            Assert.AreEqual(att["country"], group.Country);
            Assert.AreEqual(att["company_name"], group.CompanyName);
        }
    }
}
