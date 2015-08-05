using System;
using Maestrano.Saml;
using NUnit.Framework;

namespace Maestrano.Tests.Saml
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
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
