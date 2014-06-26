using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class App : ConfigurationSection
    {
        /// <summary>
        /// Load App configuration into a App configuration object
        /// </summary>
        /// <returns>A App configuration object</returns>
        public static App Load()
        {
            return ConfigurationManager.GetSection("maestrano/app") as App;
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
        /// The url of the application
        /// e.g: http://localhost:54326
        /// </summary>
        [ConfigurationProperty("environment", DefaultValue = "test", IsRequired = false)]
        public string Environment
        {
            get { return (String)this["environment"]; }
            set { this["environment"] = value; }
        }

        /// <summary>
        /// The url of the application
        /// e.g: http://localhost:54326
        /// </summary>
        [ConfigurationProperty("host", DefaultValue = "localhost", IsRequired = false)]
        public string Host 
        {
            get { return (String)this["host"]; }
            set { this["host"] = value; }
        }
    }
}
