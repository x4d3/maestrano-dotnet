using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class Connec : ConfigurationSection
    {

        /// <summary>
        /// Load configuration into a Connec configuration object
        /// </summary>
        /// <returns>A Connec configuration object</returns>
        public static Connec Load(string preset = "maestrano")
        {
            var config = ConfigurationManager.GetSection(preset + "/connec") as Connec;
            if (config == null) config = new Connec();

            return config;
        }

        /// <summary>
        /// Return the Connec! API Host
        /// </summary>
        [ConfigurationProperty("host", DefaultValue = null, IsRequired = false)]
        public string Host
        {
            get
            {
                var _host = (String)this["host"];
                if (string.IsNullOrEmpty(_host))
                {
                    if (MnoHelper.isProduction())
                    {
                        return "https://api-connec.maestrano.com";
                    }
                    else
                    {
                        return "http://api-sandbox.maestrano.io";
                    }
                }
                return _host;
            }

            set { this["host"] = value; }
        }

        /// <summary>
        /// Return the Connec! API Base Path
        /// </summary>
        [ConfigurationProperty("base-path", DefaultValue = null, IsRequired = false)]
        public string BasePath
        {
            get
            {
                var _path = (String)this["base-path"];
                if (string.IsNullOrEmpty(_path))
                {
                    if (MnoHelper.isProduction())
                    {
                        return "/api/v2";
                    }
                    else
                    {
                        return "/connec/api/v2";
                    }
                }
                return _path;
            }

            set { this["base-path"] = value; }
        }
    }
}
