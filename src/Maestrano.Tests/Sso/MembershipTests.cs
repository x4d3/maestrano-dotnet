using System;
using Maestrano.Sso;
using Maestrano.Saml;
using NUnit.Framework;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{
    [TestFixture]
    public class MembershipTests
    {
        [Test]
        public void ItShouldExtractTheRightAttributesFromTheSamlResponse()
        {
            SsoResponseStub samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var membership = new Membership(samlResp);

            Assert.AreEqual(att["uid"], membership.UserUid);
            Assert.AreEqual(att["group_uid"], membership.GroupUid);
            Assert.AreEqual(att["group_role"], membership.Role);
        }
    }
}
