using System;
using Maestrano.Saml;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using System.Text;
using Maestrano.Configuration;

namespace Maestrano.Tests.Saml
{
    [TestFixture]
    public class CertificateTests
    {

        [Test]
        public void itLoadsACertificateFromByteArray()
        {
            // Build certificate
            ASCIIEncoding ascii = new ASCIIEncoding();
            var bytCert = ascii.GetBytes(Helpers.CERTIFICATE);
            X509Certificate2 cert = new X509Certificate2(bytCert);

            // Create SAML x509 certificate from byte array
            Certificate samlCert = new Certificate(bytCert);

            Assert.IsTrue(cert.Equals(samlCert.Cert));
        }

        [Test]
        public void ItLoadsACertificateFromString()
        {
            // Build certificate
            ASCIIEncoding ascii = new ASCIIEncoding();
            var bytCert = ascii.GetBytes(Helpers.CERTIFICATE);
            X509Certificate2 cert = new X509Certificate2(bytCert);

            // Create SAML x509 certificate from string
            Certificate samlCert = new Certificate(Helpers.CERTIFICATE);
        
            Assert.IsTrue(cert.Equals(samlCert.Cert));
        }
    }
}
