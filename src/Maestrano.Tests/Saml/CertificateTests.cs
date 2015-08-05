using System;
using Maestrano.Saml;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

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
            byte[] bytCert = new byte[strCert.Length * sizeof(char)];
            System.Buffer.BlockCopy(strCert.ToCharArray(), 0, bytCert, 0, bytCert.Length);
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(bytCert);

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
            byte[] bytCert = new byte[strCert.Length * sizeof(char)];
            System.Buffer.BlockCopy(strCert.ToCharArray(), 0, bytCert, 0, bytCert.Length);
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(bytCert);

            // Create SAML x509 certificate from string
            Certificate samlCert = new Certificate();
            samlCert.LoadCertificate(strCert);

            Assert.IsTrue(cert.Equals(samlCert.cert));
        }
    }
}
