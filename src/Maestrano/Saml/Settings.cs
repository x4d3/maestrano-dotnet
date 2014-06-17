using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Saml
{
    public class Settings
    {
        public string AssertionConsumerServiceUrl { get; set; }
        public string Issuer { get; set; }
        public string IdpSsoTargetUrl { get; set; }
        public string IdpCertificate { get; set; }
        public string NameIdentifierFormat { get; set; }
    }
}
