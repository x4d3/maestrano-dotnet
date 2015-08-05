using System;
using Maestrano.Saml;
using NUnit.Framework;
using System.Text;
using System.IO;

namespace Maestrano.Tests.Saml
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void ItShouldConsiderResponse1AsInvalid()
        {
            MnoHelper.Environment = "production";

            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Response resp = new Response();
            resp.SamlCertificate().LoadCertificate(Helpers.ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsFalse(resp.IsValid());
        }

        [Test]
        public void ItShouldLoadResponseWithSpecialNewlineCharacters()
        {
            // Response2 contains \n and \r characters that should break base64.decode usually

            MnoHelper.Environment = "production";

            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response2.xml.base64");
            Response resp = new Response();
            resp.SamlCertificate().LoadCertificate(Helpers.ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsFalse(resp.IsValid());
            Assert.AreEqual("wibble@wibble.com", resp.GetNameID());
        }

        [Test]
        public void ItShouldLoadResponse4Properly()
        {
            MnoHelper.Environment = "production";

            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response4.xml.base64");
            Response resp = new Response();
            resp.SamlCertificate().LoadCertificate(Helpers.ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsTrue(resp.IsValid());
            Assert.AreEqual("bogus@onelogin.com", resp.GetNameID());
        }


        [Test]
        public void ItShouldLoadTheResponseAttributesProperly()
        {
            MnoHelper.Environment = "production";

            // Prepare response
            string samlResponse = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Response resp = new Response();
            resp.SamlCertificate().LoadCertificate(Helpers.ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.AreEqual("demo", resp.GetAttributes().Get("uid"));
            Assert.AreEqual("value", resp.GetAttributes().Get("another_value"));
        }
    }
}
