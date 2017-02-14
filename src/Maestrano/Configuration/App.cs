using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class App
    {
        /// <summary>
        /// Load configuration into a App configuration object from a JObject 
        /// </summary>
        /// <returns>A App configuration object</returns>
        public static App LoadFromJson(JObject obj)
        {
            var config = new App();
            config.Host = obj["host"].Value<string>();
            return config;
        }

        /// <summary>
        /// The url of the application
        /// e.g: http://localhost:54326
        /// </summary>
        public String Host { get; set; }
    }
}
