using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class WebhookConnec
    {
        public WebhookConnecSubscriptions Subscriptions { get; set; }

        /// <summary>
        /// Load Webhook into a WebhookAccount configuration object from a JObject 
        /// </summary>
        /// <returns>A WebhookAccount configuration object</returns>
        public static WebhookConnec LoadFromJson(JObject obj)
        {
            var config = new WebhookConnec();
            config.NotificationsPath = obj["notification_path"].Value<string>();
            var subscriptions = obj["connec_subscriptions"];
            if (subscriptions != null)
            {
                config.Subscriptions = WebhookConnecSubscriptions.LoadFromJson(subscriptions.Value<JObject>());
            }
            else
            {
                config.Subscriptions = new WebhookConnecSubscriptions();
            }

            
            return config;
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
       public string NotificationsPath { get; set; }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
       public string InitializationPath { get; set; }

    }
}
