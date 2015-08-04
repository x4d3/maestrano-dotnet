using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Collections.Specialized;

namespace Maestrano.Saml
{
    public class Response
    {
        private string presetName;
        public XmlDocument XmlDoc { get; private set; }
        public Settings settings { get; private set; }
        private Certificate certificate;
        protected NameValueCollection _cachedAttributes;

        public Response()
        {
            this.New();
        }

        /// <summary>
        /// Scope a Request to a specific configuration preset
        /// </summary>
        /// <param name="presetName"></param>
        /// <returns></returns>
        public static Response With(string presetName = "maestrano")
        {
            Response scopedResponse = new Response();
            scopedResponse.presetName = presetName;

            return scopedResponse;
        }

        /// <summary>
        /// Initialize a new Response
        /// </summary>
        /// <returns></returns>
        public Response New()
        {
            certificate = null;
            return this;
        }

        /// <summary>
        /// Return the Certificate used by the Response object
        /// </summary>
        /// <returns></returns>
        public Certificate SamlCertificate()
        {
            if (certificate == null)
            {
                certificate = new Certificate();
                certificate.LoadCertificate(MnoHelper.With(presetName).Sso.SamlSettings().IdpCertificate);
            }

            return certificate;
        }

        /// <summary>
        /// Load a XML document in string format
        /// </summary>
        /// <param name="xml"></param>
        public void LoadXml(string xml)
        {
            XmlDoc = new XmlDocument();
            XmlDoc.PreserveWhitespace = true;
            XmlDoc.XmlResolver = null;
            XmlDoc.LoadXml(xml);
        }

        /// <summary>
        /// Load a base64 encoded XML document
        /// </summary>
        /// <param name="response"></param>
        public void LoadXmlFromBase64(string response)
        {
            //Sanitize the response
            string base64Response = response;

            // Remove newline characters
            base64Response = base64Response.Replace("\n", String.Empty).Replace("\r", String.Empty).Replace("\t", String.Empty);
            
            // Pad the string with '=' to make sure the length is right
            int mod4 = base64Response.Length % 4;
            if (mod4 > 0)
                base64Response += new String('=', 4 - mod4);

            // Decode the response and load the XML document
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            LoadXml(enc.GetString(Convert.FromBase64String(base64Response)));
        }

        /// <summary>
        /// Check whether the response is valid or not based on the
        /// response certificate value and certificate used by the Response
        /// object
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            bool status = false;

            XmlNamespaceManager manager = new XmlNamespaceManager(XmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            XmlNodeList nodeList = XmlDoc.SelectNodes("//ds:Signature", manager);

            SignedXml signedXml = new SignedXml(XmlDoc);
            foreach (XmlNode node in nodeList)
            {
                try {
                    signedXml.LoadXml((XmlElement)node);
                    status = signedXml.CheckSignature(SamlCertificate().cert, true);
                } catch (System.FormatException){
                    status = false;
                }
                
                if (!status)
                    return false;
            }
            return status;
        }

        /// <summary>
        /// Retrieve the user nameid from the response
        /// </summary>
        /// <returns></returns>
        public string GetNameID()
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(XmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

            XmlNode node = XmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", manager);
            return node.InnerText;
        }

        public NameValueCollection GetAttributes()
        {
            if (_cachedAttributes != null)
            {
                return _cachedAttributes;
            }

            _cachedAttributes = new NameValueCollection();

            XmlNamespaceManager manager = new XmlNamespaceManager(XmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

            XmlNodeList nodeList = XmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute", manager);

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes != null)
                {
                    var nameAttribute = node.Attributes["Name"];
                    
                    if (nameAttribute != null)
                    {
                        string attrName = node.Attributes["Name"].Value;
                        string attrValue = null;

                        var attrValueNode = node.SelectSingleNode("saml:AttributeValue",manager);
                        if (attrValueNode != null)
                        {
                            attrValue = attrValueNode.InnerText;
                        }

                        _cachedAttributes.Add(attrName, attrValue);
                    }
                }
            }

            return _cachedAttributes;
        }
    }
}
