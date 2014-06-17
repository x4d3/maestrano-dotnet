using System;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maestrano.Tests.Saml
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void ItInitializesASettingsObjectProperly()
        {
            Settings settings = new Settings();
            settings.AssertionConsumerServiceUrl = "https://mysuperapp.com/maestrano/auth/saml/consume";
            settings.IdpCertificate = "somecertificateinfo";
            settings.IdpSsoTargetUrl = "https://maestrano.com/auth/saml";
            settings.Issuer = "my-app";
        }
    }
}
