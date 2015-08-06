using System;
using Maestrano.Sso;
using Maestrano.Saml;
using NUnit.Framework;
using System.Collections.Specialized;
using Maestrano.Helpers;

namespace Maestrano.Tests.Sso
{
    [TestFixture]
    public class GroupTests
    {
        [Test]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            SsoResponseStub samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var group = new Group(samlResp);

            Assert.AreEqual(att["group_uid"], group.Uid);
            Assert.AreEqual(att["group_name"], group.Name);
            Assert.AreEqual(att["group_email"], group.Email);
            Assert.AreEqual(DateTime.Parse(att["group_end_free_trial"]), group.FreeTrialEndAt);
            Assert.AreEqual(att["company_name"], group.CompanyName);
            Assert.AreEqual(att["group_has_credit_card"].Equals("true"), group.HasCreditCard);

            Assert.AreEqual(att["group_currency"], group.Currency);
            Assert.AreEqual(TimeZoneConverter.fromOlsonTz(att["group_timezone"]), group.Timezone);
            Assert.AreEqual(att["group_country"], group.Country);
            Assert.AreEqual(att["group_city"], group.City);
        }
    }
}
