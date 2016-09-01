using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class WebhookAccount : ConfigurationSection
    {
        /// <summary>
        /// Load WebhooAccount configuration into a WebhooAccount configuration object
        /// </summary>
        /// <returns>A WebhooAccount configuration object</returns>
        public static WebhookAccount Load(string preset = "maestrano")
        {
            ConfigurationManager.RefreshSection(preset + "/webhook/account");
            var config = ConfigurationManager.GetSection(preset + "/webhook/account") as WebhookAccount;
            if (config == null) config = new WebhookAccount();

            return config;
        }

        /// <summary>
        /// Load Webhook into a WebhookAccount configuration object from a JObject 
        /// </summary>
        /// <returns>A WebhookAccount configuration object</returns>
        public static WebhookAccount LoadFromJson(String preset, JObject obj)
        {
            var config = new WebhookAccount();
            config.GroupsPath = obj["group_path"].Value<string>();
            config.GroupUsersPath = obj["group_user_path"].Value<string>();
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
        /// Application REST endpoint for groups
        /// </summary>
        [ConfigurationProperty("groupsPath", DefaultValue = "/maestrano/account/groups/:id", IsRequired = false)]
        public String GroupsPath
        {
            get { return (String)this["groupsPath"]; }
            set { this["groupsPath"] = value; }
        }

        /// <summary>
        /// Application REST endpoint for group > users
        /// </summary>
        [ConfigurationProperty("groupUsersPath", DefaultValue = "/maestrano/account/groups/:group_id/users/:id", IsRequired = false)]
        public String GroupUsersPath
        {
            get { return (String)this["groupUsersPath"]; }
            set { this["groupUsersPath"] = value; }
        }
    }
}
