using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    class Sso
    {
        // Is Single Sign-On enabled - useful for debugging
        public bool Enabled { get; set; }

        // SSO user creation mode: 'real' or 'virtual'
        public string CreationMode { get; set; }

        // Path to init action
        public string InitPath { get; set; }

        // Path to consume action
        public string ConsumePath { get; set; }

        // Address of the identity provider
        public string Idp { get; set; }

        // The nameid format for SAML handshake
        public string NameIdFormat { get; set; }

        // Fingerprint of x509 certificate used for SAML
        public string X509Fingerprint { get; set; }

        // Actual x509 certificate
        public string X509Certificate { get; set; }

    }
}
