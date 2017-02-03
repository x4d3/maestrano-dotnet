using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Collections.Specialized;
using Maestrano.Configuration;
using System.Text.RegularExpressions;

namespace Maestrano.Saml
{
    public class Response
    {
        private Certificate certificate;
	    private String xml;
	    private XmlDocument xmlDoc;

        public Settings settings { get; private set; }
        protected NameValueCollection _cachedAttributes;

        public static Response LoadFromXML(Preset preset, String xml)
        {
            return LoadFromXML(preset.Sso, xml);
        }

        public static Response LoadFromBase64XML(Preset preset, String xml)
        {
            return LoadFromXML(preset.Sso, xml);
        }

        public static Response LoadFromXML(Configuration.Sso sso, String xml)
        {
            return new Response(sso.SamlSettings().IdpCertificate, xml);
        }
        
        public static Response LoadFromBase64XML(Configuration.Sso sso, String response)
        {
            //Sanitize the response
            // Remove newline characters
            string base64Response = Regex.Replace(response, @"\t|\n|\r", "");

            // Pad the string with '=' to make sure the length is right
            int mod4 = base64Response.Length % 4;
            if (mod4 > 0)
                base64Response += new String('=', 4 - mod4);

            // Decode the response and load the XML document
            string xml = Encoding.ASCII.GetString(Convert.FromBase64String(base64Response));

            return LoadFromXML(sso, xml);
        }

        /// <summary>
        /// Only for tests
        /// </summary>
        protected Response()
        {
        }

        /// <summary>
        /// Initialize a new Response
        /// </summary>
        public Response(String idCertificate, String xml)
        {
            this.xml = xml;
            this.xmlDoc = loadXml(xml);
            this.certificate = new Certificate(idCertificate);
        }

        /// <summary>
        /// Return the Certificate used by the Response object
        /// </summary>
        /// <returns></returns>
        public Certificate SamlCertificate()
        {
            return certificate;
        }

        /// <summary>
        /// Load a XML document in string format
        /// </summary>
        /// <param name="xml"></param>
        private static XmlDocument loadXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.XmlResolver = null;
            xmlDoc.LoadXml(xml);
            return xmlDoc;
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

            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            XmlNodeList nodeList = xmlDoc.SelectNodes("//ds:Signature", manager);

            SignedXml signedXml = new SignedXml(xmlDoc);
            foreach (XmlNode node in nodeList)
            {
                try {
                    signedXml.LoadXml((XmlElement)node);
                    status = signedXml.CheckSignature(SamlCertificate().Cert, true);
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
            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

            XmlNode node = xmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", manager);
            return node.InnerText;
        }

        public NameValueCollection GetAttributes()
        {
            if (_cachedAttributes != null)
            {
                return _cachedAttributes;
            }
            _cachedAttributes = new NameValueCollection();

            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

            XmlNodeList nodeList = xmlDoc.SelectNodes("/samlp:Response/saml:Assertion/saml:AttributeStatement/saml:Attribute", manager);

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
