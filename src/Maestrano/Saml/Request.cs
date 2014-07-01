using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;

namespace Maestrano.Saml
{
    public class Request
    {
        public string Id {get; private set;}
        public string IssueInstant { get; private set; }

        public Settings settings { get; private set; }
        public NameValueCollection parameters { get; private set; }

        public enum RequestFormat
        {
            Base64 = 1   
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameters">The request GET parameters typically obtained via HttpRequest.Params</param>
        public Request(NameValueCollection parameters = null)
        {
            this.parameters = parameters;
            this.settings = Mno.Sso.SamlSettings();

            Id = "_" + Guid.NewGuid().ToString();
            IssueInstant = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }


        /// <summary>
        /// Return the SAML XML request, Base64 encoded
        /// </summary>
        /// <returns></returns>
        public string GetXmlBase64Request()
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", Id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", IssueInstant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", settings.AssertionConsumerServiceUrl);

                        xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                            xw.WriteString(settings.Issuer);
                        xw.WriteEndElement();

                        xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                        xw.WriteAttributeString("Format", settings.NameIdentifierFormat);
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


        /// <summary>
        /// Return the SAML request redirection URL that initiates
        /// the Single-Sign On handshake on the Identity Provider side
        /// </summary>
        /// <returns>String URL</returns>
        public string RedirectUrl()
        {
            // Build the base url
            string url = settings.IdpSsoTargetUrl;
            url += "?SAMLRequest=";
            url += HttpUtility.UrlEncode(this.GetXmlBase64Request());

            // Add query parameters
            if (parameters != null)
                foreach (String k in parameters.AllKeys)
                    url += "&" + k + "=" + HttpUtility.UrlEncode(parameters[k]);

            return url;
        }
    }
}
