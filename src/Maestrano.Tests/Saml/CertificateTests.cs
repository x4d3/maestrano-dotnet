using System;
using Maestrano.Saml;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using System.Text;

namespace Maestrano.Tests.Saml
{
    [TestFixture]
    public class CertificateTests
    {
        [Test]
        public void itLoadsACertificateFromByteArray()
        {
            MnoHelper.Environment = "production";
            string strCert = MnoHelper.Sso.X509Certificate;

            // Build certificate
            ASCIIEncoding ascii = new ASCIIEncoding();
            var bytCert = ascii.GetBytes(strCert);
            X509Certificate2 cert = new X509Certificate2(bytCert);

            // Create SAML x509 certificate from byte array
            Certificate samlCert = new Certificate();
            samlCert.LoadCertificate(bytCert);

            Assert.IsTrue(cert.Equals(samlCert.cert));
        }

        [Test]
        public void ItLoadsACertificateFromString()
        {
            MnoHelper.Environment = "production";
            string strCert = MnoHelper.Sso.X509Certificate;

            // Build certificate
            ASCIIEncoding ascii = new ASCIIEncoding();
            var bytCert = ascii.GetBytes(strCert);
            X509Certificate2 cert = new X509Certificate2(bytCert);

            // Create SAML x509 certificate from string
            Certificate samlCert = new Certificate();
            samlCert.LoadCertificate(strCert);

            Assert.IsTrue(cert.Equals(samlCert.cert));
        }
    }
}
