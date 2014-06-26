using System;
using Maestrano.Sso;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Maestrano.Tests.Sso
{
    [TestClass]
    public class MembershipTests
    {
        [TestMethod]
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
