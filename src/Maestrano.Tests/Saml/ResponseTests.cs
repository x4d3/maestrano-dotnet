using System;
using Maestrano.Saml;
using NUnit.Framework;
using System.Text;
using System.IO;
using Maestrano.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Maestrano.Tests.Saml
{
    [TestFixture]
    public class ResponseTests
    {
        const String CERTIFICATE = "-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAPFpcH2rW0pyMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyNDEw\nWhcNMzMxMjMwMDUyNDEwWjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD3feNNn2xfEz5/\nQvkBIu2keh9NNhobpre8U4r1qC7h7OeInTldmxGL4cLHw4ZAqKbJVrlFWqNevM5V\nZBkDe4mjuVkK6rYK1ZK7eVk59BicRksVKRmdhXbANk/C5sESUsQv1wLZyrF5Iq8m\na9Oy4oYrIsEF2uHzCouTKM5n+O4DkwIDAQABo4HuMIHrMB0GA1UdDgQWBBSd/X0L\n/Pq+ZkHvItMtLnxMCAMdhjCBuwYDVR0jBIGzMIGwgBSd/X0L/Pq+ZkHvItMtLnxM\nCAMdhqGBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA8WlwfatbSnIwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQDE\nhe/18oRh8EqIhOl0bPk6BG49AkjhZZezrRJkCFp4dZxaBjwZTddwo8O5KHwkFGdy\nyLiPV326dtvXoKa9RFJvoJiSTQLEn5mO1NzWYnBMLtrDWojOe6Ltvn3x0HVo/iHh\nJShjAn6ZYX43Tjl1YXDd1H9O+7/VgEWAQQ32v8p5lA==\n-----END CERTIFICATE-----";

        Preset preset;

        [TestFixtureSetUp]
        public void TestInitialize()
        {
            Helpers.destroyMnoSession();
            preset = new Preset("test");
            preset.Sso.X509Certificate = CERTIFICATE;
        }

        [Test]
        public void ItShouldLoadResponseWithSpecialNewlineCharacters()
        {
            // Response2 contains \n and \r characters that should break base64.decode usually
            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response2.xml.base64");
            Response resp = Response.LoadFromBase64XML(preset.Sso, samlResponse);
            resp.SamlCertificate().Cert = loadCertificate("Certificates/certificate1");
            Assert.IsFalse(resp.IsValid());
            Assert.AreEqual("wibble@wibble.com", resp.GetNameID());
        }

        [Test]
        public void ItShouldLoadResponse4Properly()
        {
            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response4.xml.base64");
            Response resp = Response.LoadFromBase64XML(preset.Sso, samlResponse);
            resp.SamlCertificate().Cert = loadCertificate("Certificates/certificate1");

            Assert.IsTrue(resp.IsValid());
            Assert.AreEqual("bogus@onelogin.com", resp.GetNameID());
        }


        [Test]
        public void ItShouldLoadTheResponseAttributesProperly()
        {
            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Response resp = Response.LoadFromBase64XML(preset.Sso, samlResponse);
            resp.SamlCertificate().Cert = loadCertificate("Certificates/certificate1");

            Assert.AreEqual("demo", resp.GetAttributes().Get("uid"));
            Assert.AreEqual("value", resp.GetAttributes().Get("another_value"));
        }

        private X509Certificate2 loadCertificate(String path)
        {
            var certificate = Helpers.ReadSamlSupportFiles(path);
            return new X509Certificate2(Encoding.ASCII.GetBytes(certificate));
        }

    }
}
