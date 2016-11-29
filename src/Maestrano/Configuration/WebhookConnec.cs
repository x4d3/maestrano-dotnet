using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class WebhookConnec : ConfigurationSection
    {

        public WebhookConnecSubscriptions Subscriptions { get; set; }


        /// <summary>
        /// Load WebhookConnec configuration into a WebhookConnec configuration object
        /// </summary>
        /// <returns>A WebhooAccount configuration object</returns>
        public static WebhookConnec Load(string preset = "maestrano")
        {
            ConfigurationManager.RefreshSection(preset + "/webhook/connec");
            var config =  ConfigurationManager.GetSection(preset + "/webhook/connec") as WebhookConnec;
            if (config == null) config = new WebhookConnec();
            config.Subscriptions = WebhookConnecSubscriptions.Load();

            return config;
        }

        /// <summary>
        /// Load Webhook into a WebhookAccount configuration object from a JObject 
        /// </summary>
        /// <returns>A WebhookAccount configuration object</returns>
        public static WebhookConnec LoadFromJson(String preset, JObject obj)
        {
            var config = new WebhookConnec();
            config.NotificationsPath = obj["notification_path"].Value<string>();
            if (obj["connec_subscriptions"] == null)
            {
                config.Subscriptions = WebhookConnecSubscriptions.Load();
            }
            else
            {
                config.Subscriptions = WebhookConnecSubscriptions.LoadFromJson(preset, obj["connec_subscriptions"].Value<JObject>());
            }
            
            return config;
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("notificationsPath", DefaultValue = "/maestrano/connec/notifications", IsRequired = false)]
        public string NotificationsPath
        {
            get { return (String)this["notificationsPath"]; }
            set { this["notificationsPath"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("notificationsPath", DefaultValue = "/maestrano/connec/notifications", IsRequired = false)]
        public string InitializationPath
        {
            get { return (String)this["initializationPath"]; }
            set { this["initializationPath"] = value; }
        }

    }
}
