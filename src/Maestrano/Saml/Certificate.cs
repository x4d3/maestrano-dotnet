using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography.X509Certificates;

namespace Maestrano.Saml
{
    public class Certificate
    {
        public X509Certificate2 Cert { get; set; }
        /// <summary>
        /// Load a x509 certificate from byte array
        /// </summary>
        /// <param name="certificate">Certificate in byte array format</param>
        public Certificate(byte[] certificateByteArray)
        {
            Cert = new X509Certificate2(certificateByteArray);
        }

        /// <summary>
        /// Load a x509 certificate from string
        /// </summary>
        /// <param name="certificate">Certificate in string format</param>
        public Certificate(string certificate)
        {
            byte[] certificateByteArray = Encoding.ASCII.GetBytes(certificate);
            Cert = new X509Certificate2(certificateByteArray);
        }
    }
}
