using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class Api
    {
        // API Id (from sandbox or maestrano.com)
        public string Id { get; set; }

        // API Key (from sandbox or maestrano.com)
        public string Key { get; set; }

        // Return an identification token ready to
        // be used for HTTP auth
        public string Token { get { return Id + ':' + Key; } }

        // Language used by the API
        public string Lang { get { return "C#"; } }

        // Return the API version
        public string Version { get { return Maestrano.Version; } }

        // Return the language version
        public string LangVersion { get { return Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(); } }

        // Verify SSL certs (disabled for now)
        public bool VerifySslCerts { get { return false; } }
    }
}
