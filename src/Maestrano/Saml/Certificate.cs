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
        public X509Certificate2 cert;

        /// <summary>
        /// Load a x509 certificate from byte array
        /// </summary>
        /// <param name="certificate">Certificate in byte array format</param>
        public void LoadCertificate(byte[] certificate)
        {
            cert = new X509Certificate2();
            cert.Import(certificate);
        }

        /// <summary>
        /// Load a x509 certificate from string
        /// </summary>
        /// <param name="certificate">Certificate in string format</param>
        public void LoadCertificate(string certificate)
        {
            LoadCertificate(StringToByteArray(certificate));
        }

        /// <summary>
        /// Convert a string to byte array
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        private byte[] StringToByteArray(string st)
        {
            byte[] byteArray = new byte[st.Length * sizeof(char)];
            System.Buffer.BlockCopy(st.ToCharArray(), 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
    }
}
