using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class DevPlatformEnvironment : ConfigurationElement
    {

        /// <summary>
        /// Return False (object not read only)
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// The environment Nid
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = null, IsRequired = false)]
        public String Name
        {
            get { return (String)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// The environment Api Key
        /// </summary>
        [ConfigurationProperty("apiKey", DefaultValue = null, IsRequired = false)]
        public String ApiKey
        {
            get { return (String)this["apiKey"]; }
            set { this["apiKey"] = value; }
        }


        /// <summary>
        /// The environment Api Secret
        /// </summary>
        [ConfigurationProperty("apiSecret", DefaultValue = null, IsRequired = false)]
        public String ApiSecret
        {
            get { return (String)this["apiSecret"]; }
            set { this["apiSecret"] = value; }
        }
    }
}
