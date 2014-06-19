using System;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;

namespace Maestrano.Tests.Saml
{
    [TestClass]
    public class ResponseTests
    {
        // Read a reponse from the support/saml folder
        private string ReadSamlSupportFiles(string responseName)
        {
            // Build path
            string responseFolder = "../../Support/Saml";
            string responsePath = responseFolder + "/" + responseName;

            // Read the file
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(responsePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string result = sb.ToString();

            // Return as string
            return sb.ToString();
        }

        [TestMethod]
        public void ItShouldConsiderResponse1AsInvalid()
        {
            Maestrano.Environment = "production";

            string samlResponse = ReadSamlSupportFiles("Responses/response1.xml.base64");
            Response resp = new Response();
            resp.Cert.LoadCertificate(ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsFalse(resp.IsValid());
        }

        [TestMethod]
        public void ItShouldLoadResponse2ButConsiderItAsInvalid()
        {
            // Response2 contains \n and \r characters that should break base64.decode usually

            Maestrano.Environment = "production";
            string samlResponse = ReadSamlSupportFiles("Responses/response2.xml.base64");

            Response resp = new Response();
            resp.Cert.LoadCertificate(ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsFalse(resp.IsValid());
            Assert.AreEqual("wibble@wibble.com", resp.GetNameID());
        }

        [TestMethod]
        public void ItShouldLoadResponse4Properly()
        {
            // Response4 is valid and formatted the way a web app
            // would do it

            Maestrano.Environment = "production";

            string samlResponse = ReadSamlSupportFiles("Responses/response4.xml.base64");
            Response resp = new Response();
            resp.Cert.LoadCertificate(ReadSamlSupportFiles("Certificates/certificate1"));
            resp.LoadXmlFromBase64(samlResponse);

            Assert.IsTrue(resp.IsValid());
            Assert.AreEqual("bogus@onelogin.com", resp.GetNameID());
        }
    }
}
