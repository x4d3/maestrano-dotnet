using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Web;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maestrano.Tests.Saml
{
    [TestClass]
    public class RequestTests
    {
        

        [TestMethod]
        public void GetRequest_ItReturnsTheRightBased64EncodedXmlRequest()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";
            Maestrano.Api.Id = "app-1";

            Request req = new Request();

            Assert.AreEqual(XmlRequestBase64Encoded(req.Id, req.IssueInstant), req.GetXmlBase64Request());
        }

        [TestMethod]
        public void RedirectUrl_WithNoParameters_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";
            Maestrano.Api.Id = "app-1";

            // Build parameters
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("group_uid","someparamvalue");
            parameters.Add("someotherparam","someothervalue");

            // Build request
            Request req = new Request(parameters);

            // Build expected url
            string expectedUrl = Maestrano.Sso.IdpUrl();
            expectedUrl += "?SAMLRequest=";
            expectedUrl += HttpUtility.UrlEncode(XmlRequestBase64Encoded(req.Id, req.IssueInstant));
            expectedUrl += "&group_uid=someparamvalue";
            expectedUrl += "&someotherparam=someothervalue";

            Assert.AreEqual(expectedUrl, req.RedirectUrl());
        }

        [TestMethod]
        public void RedirectUrl_WithParameters_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";
            Maestrano.Api.Id = "app-1";

            // Build request
            Request req = new Request();

            // Build expected url
            string expectedUrl = Maestrano.Sso.IdpUrl();
            expectedUrl += "?SAMLRequest=";
            expectedUrl += HttpUtility.UrlEncode(XmlRequestBase64Encoded(req.Id, req.IssueInstant));

            Assert.AreEqual(expectedUrl, req.RedirectUrl());
        }


        /// <summary>
        /// Simulate a Base64 encoded XML Request
        /// </summary>
        /// <param name="id"></param>
        /// <param name="issueInstant"></param>
        /// <param name="consumeUrl"></param>
        /// <param name="issuer"></param>
        /// <param name="nameIdFormat"></param>
        /// <returns></returns>
        public string XmlRequestBase64Encoded(string id, string issueInstant, string consumeUrl = null, string issuer = null, string nameIdFormat = null)
        {
            // Default values
            if (string.IsNullOrEmpty(consumeUrl))
                consumeUrl = Maestrano.Sso.ConsumeUrl();
            if (string.IsNullOrEmpty(nameIdFormat))
                nameIdFormat = Maestrano.Sso.NameIdFormat;
            if (string.IsNullOrEmpty(issuer))
                issuer = Maestrano.Api.Id;

            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", issueInstant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", consumeUrl);

                        xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                            xw.WriteString(issuer);
                        xw.WriteEndElement();

                        xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("Format", nameIdFormat);
                        xw.WriteAttributeString("AllowCreate", "true");
                        xw.WriteEndElement();

                        xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("Comparison", "exact");

                            xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                                xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                            xw.WriteEndElement();

                        xw.WriteEndElement();

                    xw.WriteEndElement();
                }

                // Encode in base64
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                return System.Convert.ToBase64String(toEncodeAsBytes);
            }
        }
    }
}
