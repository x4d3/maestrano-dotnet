using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class Connec
    {

        /// <summary>
        /// Load configuration into a Connec configuration object from a JObject 
        /// </summary>
        /// <returns>A Connec configuration object</returns>
        public static Connec LoadFromJson(JObject obj)
        {
            var config = new Connec();
            config.Host = obj["host"].ToString();
            config.BasePath = obj["base_path"].ToString();
            return config;
        }

        /// <summary>
        /// Return the Connec! API Host
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// Return the Connec! API Base Path
        /// </summary>
        public String BasePath { get; set; }

    }
}
