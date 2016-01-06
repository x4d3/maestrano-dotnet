using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("notificationsPath", DefaultValue = "/maestrano/connec/notifications", IsRequired = false)]
        public string NotificationsPath
        {
            get { return (String)this["notificationsPath"]; }
            set { this["notificationsPath"] = value; }
        }
    }
}
