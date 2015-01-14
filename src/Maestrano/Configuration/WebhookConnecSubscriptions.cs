using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class WebhookConnecSubscriptions : ConfigurationSection
    {
        /// <summary>
        /// Load WebhookConnec configuration into a WebhookConnec configuration object
        /// </summary>
        /// <returns>A WebhooAccount configuration object</returns>
        public static WebhookConnecSubscriptions Load()
        {
            var config = ConfigurationManager.GetSection("maestrano/webhook/connec-subscriptions") as WebhookConnecSubscriptions;
            if (config == null) config = new WebhookConnecSubscriptions();

            return config;
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("company", DefaultValue = false, IsRequired = false)]
        public bool Company
        {
            get { return (Boolean)this["company"]; }
            set { this["company"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to organizations
        /// </summary>
        [ConfigurationProperty("organizations", DefaultValue = false, IsRequired = false)]
        public bool Organizations
        {
            get { return (Boolean)this["organizations"]; }
            set { this["organizations"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to people
        /// </summary>
        [ConfigurationProperty("people", DefaultValue = false, IsRequired = false)]
        public bool People
        {
            get { return (Boolean)this["people"]; }
            set { this["people"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to items
        /// </summary>
        [ConfigurationProperty("items", DefaultValue = false, IsRequired = false)]
        public bool Items
        {
            get { return (Boolean)this["items"]; }
            set { this["items"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to accounts
        /// </summary>
        [ConfigurationProperty("accounts", DefaultValue = false, IsRequired = false)]
        public bool Accounts
        {
            get { return (Boolean)this["accounts"]; }
            set { this["accounts"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to tax rates
        /// </summary>
        [ConfigurationProperty("tax-rates", DefaultValue = false, IsRequired = false)]
        public bool TaxRates
        {
            get { return (Boolean)this["tax-rates"]; }
            set { this["tax-rates"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to tax codes
        /// </summary>
        [ConfigurationProperty("tax-codes", DefaultValue = false, IsRequired = false)]
        public bool TaxCodes
        {
            get { return (Boolean)this["tax-codes"]; }
            set { this["tax-codes"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to invoices
        /// </summary>
        [ConfigurationProperty("invoices", DefaultValue = false, IsRequired = false)]
        public bool Invoices
        {
            get { return (Boolean)this["invoices"]; }
            set { this["invoices"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to payments
        /// </summary>
        [ConfigurationProperty("payments", DefaultValue = false, IsRequired = false)]
        public bool Payments
        {
            get { return (Boolean)this["payments"]; }
            set { this["payments"] = value; }
        }
    }
}
