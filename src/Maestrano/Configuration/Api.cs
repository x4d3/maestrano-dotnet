using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class Api : ConfigurationSection
    {

        /// <summary>
        /// Load Api configuration into a Api configuration object
        /// </summary>
        /// <returns>A Api configuration object</returns>
        public static Api Load()
        {
            return ConfigurationManager.GetSection("maestrano/api") as Api;
        }

        /// <summary>
        /// Return False (object not read only)
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// API Id (from sandbox or maestrano.com)
        /// </summary>
        [ConfigurationProperty("id", DefaultValue = null, IsRequired = false)]
        public String Id
        {
            get { return (String)this["id"]; }
            set { this["id"] = value; }
        }

        /// <summary>
        /// API Key (from sandbox or maestrano.com)
        /// </summary>
        [ConfigurationProperty("key", DefaultValue = null, IsRequired = false)]
        public String Key
        {
            get { return (String)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Verify SSL certs (disabled for now)
        /// </summary>
        [ConfigurationProperty("verifySslCerts", DefaultValue = false, IsRequired = false)]
        public bool VerifySslCerts
        {
            get { return (Boolean)this["verifySslCerts"]; }
            set { this["verifySslCerts"] = value; }
        }

        // Return an identification token ready to
        // be used for HTTP auth
        public string Token { get { return Id + ':' + Key; } }

        // Language used by the API
        public string Lang { get { return "C#"; } }

        // Return the API version
        public string Version { get { return Maestrano.Version; } }

        // Return the language version
        public string LangVersion { get { return Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(); } }

    }
}
