using System;
using Maestrano.Sso;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            SsoResponseStub samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var group = new Group(samlResp);

            Assert.AreEqual(att["group_uid"], group.Uid);
            Assert.AreEqual(DateTime.Parse(att["group_end_free_trial"]), group.FreeTrialEndAt);
            Assert.AreEqual(att["country"], group.Country);
            Assert.AreEqual(att["company_name"], group.CompanyName);
        }
    }
}
