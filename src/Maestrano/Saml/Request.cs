using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;

namespace Maestrano.Saml
{
    class Request
    {
        public string id;
        private string issue_instant;
        private Settings _settings;

        public enum RequestFormat
        {
            Base64 = 1   
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="samlSettings">Saml Settings defining the issuer, idp, return url etc.</param>
        public Request()
        {
            this._settings = Maestrano.Sso.SamlSettings();

            id = "_" + Guid.NewGuid().ToString();
            issue_instant = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }


        /// <summary>
        /// Return the SAML XML request, Base64 encoded
        /// </summary>
        /// <returns></returns>
        public string GetRequest()
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", issue_instant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", _settings.AssertionConsumerServiceUrl);

                    xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString(_settings.Issuer);
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Format", _settings.NameIdentifierFormat);
                    xw.WriteAttributeString("AllowCreate", "true");
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Comparison", "exact");
                    xw.WriteEndElement();

                    xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                }

                // Encode in base64
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                return System.Convert.ToBase64String(toEncodeAsBytes);
            }
        }


        /// <summary>
        /// Return the SAML request redirection URL that initiates
        /// the Single-Sign On handshake on the Identity Provider side
        /// </summary>
        /// <returns>String URL</returns>
        public string RedirectUrl()
        {
            string url = _settings.IdpSsoTargetUrl;
            url += "?SAMLRequest=";
            url += System.Web.HttpUtility.UrlDecode(this.GetRequest());

            return url;
        }
    }
}
