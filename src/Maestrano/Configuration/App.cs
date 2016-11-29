using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class App : ConfigurationSection
    {
        private string presetName;
        /// <summary>
        /// Load App configuration into a App configuration object
        /// </summary>
        /// <returns>A App configuration object</returns>
        public static App Load(string preset = "maestrano")
        {
            ConfigurationManager.RefreshSection(preset + "/app");
            var config = ConfigurationManager.GetSection(preset + "/app") as App;
            if (config == null) config = new App();
            config.presetName = preset;
            return config;
        }

        /// <summary>
        /// Load configuration into a App configuration object from a JObject 
        /// </summary>
        /// <returns>A App configuration object</returns>
        public static App LoadFromJson(string preset, JObject obj)
        {
            var config = new App();
            config.presetName = preset;
            config.Host = obj["host"].Value<string>();
            return config;
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
        /// The environment of the application
        /// e.g: development
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
