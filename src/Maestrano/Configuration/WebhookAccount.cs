using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class WebhookAccount
    {
        /// <summary>
        /// Load Webhook into a WebhookAccount configuration object from a JObject 
        /// </summary>
        /// <returns>A WebhookAccount configuration object</returns>
        public static WebhookAccount LoadFromJson(JObject obj)
        {
            var config = new WebhookAccount();
            config.GroupsPath = obj["group_path"].Value<string>();
            config.GroupUsersPath = obj["group_user_path"].Value<string>();
            return config;
        }

        /// <summary>
        /// Application REST endpoint for groups
        /// </summary>
        public String GroupsPath { get; set; }

        /// <summary>
        /// Application REST endpoint for group > users
        /// </summary>
        public String GroupUsersPath { get; set; }
    }
}
