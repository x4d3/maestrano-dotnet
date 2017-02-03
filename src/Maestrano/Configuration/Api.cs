using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class Api
    {

        /// <summary>
        /// Load Api configuration into a Api configuration object from a Json Object
        /// </summary>
        /// <returns>A Api configuration object</returns>
        public static Api LoadFromJson(JObject obj)
        {
            var config = new Api();
            config.Id = obj["id"].Value<string>();
            config.Key = obj["key"].Value<string>();
            config.Base = obj["base"].Value<string>();
            config.Host = obj["host"].Value<string>();
            return config;
        }

        /// <summary>
        /// API Id
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// API Key
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        /// API Key
        /// </summary>
        [ConfigurationProperty("base", DefaultValue = "/api/v1/", IsRequired = false)]
        public String Base { get; set; }

        /// <summary>
        /// Address of the API Host
        /// </summary>
        public String Host { get; set; }

        // Return an identification token ready to
        // be used for HTTP auth
        public string Token { get { return Id + ':' + Key; } }

        // Language used by the API
        public string Lang { get { return "C#"; } }

        // Return the API version
        public string Version { get { return MnoHelper.Version; } }

        // Return the language version
        public string LangVersion { get { return Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(); } }

    }
}
