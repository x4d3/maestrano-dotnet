using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography.Xml;

namespace Maestrano.Saml
{
    class Response
    {
        public XmlDocument XmlDoc { get; private set; }
        public Settings settings { get; private set; }
        public Certificate Cert { get; private set; }

        public Response()
        {
            settings = Maestrano.Sso.SamlSettings();
            string strCert = settings.IdpCertificate;
            Cert = new Certificate();
            Cert.LoadCertificate(strCert);
        }

        public void LoadXml(string xml)
        {
            XmlDoc = new XmlDocument();
            XmlDoc.PreserveWhitespace = true;
            XmlDoc.XmlResolver = null;
            XmlDoc.LoadXml(xml);
        }

        public void LoadXmlFromBase64(string response)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            LoadXml(enc.GetString(Convert.FromBase64String(response)));
        }

        public bool IsValid()
        {
            bool status = false;

            XmlNamespaceManager manager = new XmlNamespaceManager(XmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            XmlNodeList nodeList = XmlDoc.SelectNodes("//ds:Signature", manager);

            SignedXml signedXml = new SignedXml(XmlDoc);
            foreach (XmlNode node in nodeList)
            {
                signedXml.LoadXml((XmlElement)node);
                status = signedXml.CheckSignature(Cert.cert, true);
                if (!status)
                    return false;
            }
            return status;
        }

        public string GetNameID()
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(XmlDoc.NameTable);
            manager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            manager.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            manager.AddNamespace("samlp", "urn:oasis:names:tc:SAML:2.0:protocol");

            XmlNode node = XmlDoc.SelectSingleNode("/samlp:Response/saml:Assertion/saml:Subject/saml:NameID", manager);
            return node.InnerText;
        }
    }
}
