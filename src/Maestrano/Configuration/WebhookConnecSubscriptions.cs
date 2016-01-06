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
        public static WebhookConnecSubscriptions Load(string preset = "maestrano")
        {
            ConfigurationManager.RefreshSection(preset + "/webhook/connecSubscriptions");
            var config = ConfigurationManager.GetSection(preset + "/webhook/connecSubscriptions") as WebhookConnecSubscriptions;
            if (config == null) config = new WebhookConnecSubscriptions();

            return config;
        }


        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("accounts", DefaultValue = false, IsRequired = false)]
        public bool Accounts
        {
            get { return (Boolean)this["accounts"]; }
            set { this["accounts"] = value; }
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
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("invoices", DefaultValue = false, IsRequired = false)]
        public bool Invoices
        {
            get { return (Boolean)this["invoices"]; }
            set { this["invoices"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("salesOrders", DefaultValue = false, IsRequired = false)]
        public bool SalesOrders
        {
            get { return (Boolean)this["salesOrders"]; }
            set { this["salesOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("purchaseOrders", DefaultValue = false, IsRequired = false)]
        public bool PurchaseOrders
        {
            get { return (Boolean)this["purchaseOrders"]; }
            set { this["purchaseOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("quotes", DefaultValue = false, IsRequired = false)]
        public bool Quotes
        {
            get { return (Boolean)this["quotes"]; }
            set { this["quotes"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("payments", DefaultValue = false, IsRequired = false)]
        public bool Payments
        {
            get { return (Boolean)this["payments"]; }
            set { this["payments"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("journals", DefaultValue = false, IsRequired = false)]
        public bool Journals
        {
            get { return (Boolean)this["journals"]; }
            set { this["journals"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("items", DefaultValue = false, IsRequired = false)]
        public bool Items
        {
            get { return (Boolean)this["items"]; }
            set { this["items"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("organizations", DefaultValue = false, IsRequired = false)]
        public bool Organizations
        {
            get { return (Boolean)this["organizations"]; }
            set { this["organizations"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("people", DefaultValue = false, IsRequired = false)]
        public bool People
        {
            get { return (Boolean)this["people"]; }
            set { this["people"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("projects", DefaultValue = false, IsRequired = false)]
        public bool Projects
        {
            get { return (Boolean)this["projects"]; }
            set { this["projects"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("taxCodes", DefaultValue = false, IsRequired = false)]
        public bool TaxCodes
        {
            get { return (Boolean)this["taxCodes"]; }
            set { this["taxCodes"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("taxRates", DefaultValue = false, IsRequired = false)]
        public bool TaxRates
        {
            get { return (Boolean)this["taxRates"]; }
            set { this["taxRates"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("events", DefaultValue = false, IsRequired = false)]
        public bool Events
        {
            get { return (Boolean)this["events"]; }
            set { this["events"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("venues", DefaultValue = false, IsRequired = false)]
        public bool Venues
        {
            get { return (Boolean)this["venues"]; }
            set { this["venues"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("eventOrders", DefaultValue = false, IsRequired = false)]
        public bool EventOrders
        {
            get { return (Boolean)this["eventOrders"]; }
            set { this["eventOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("workLocations", DefaultValue = false, IsRequired = false)]
        public bool WorkLocations
        {
            get { return (Boolean)this["workLocations"]; }
            set { this["workLocations"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("payItems", DefaultValue = false, IsRequired = false)]
        public bool PayItems
        {
            get { return (Boolean)this["payItems"]; }
            set { this["payItems"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("employees", DefaultValue = false, IsRequired = false)]
        public bool Employees
        {
            get { return (Boolean)this["employees"]; }
            set { this["employees"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("paySchedules", DefaultValue = false, IsRequired = false)]
        public bool PaySchedules
        {
            get { return (Boolean)this["paySchedules"]; }
            set { this["paySchedules"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("timeSheets", DefaultValue = false, IsRequired = false)]
        public bool TimeSheets
        {
            get { return (Boolean)this["timeSheets"]; }
            set { this["timeSheets"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("timeActivities", DefaultValue = false, IsRequired = false)]
        public bool TimeActivities
        {
            get { return (Boolean)this["timeActivities"]; }
            set { this["timeActivities"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("payRuns", DefaultValue = false, IsRequired = false)]
        public bool PayRuns
        {
            get { return (Boolean)this["payRuns"]; }
            set { this["payRuns"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        [ConfigurationProperty("payStubs", DefaultValue = false, IsRequired = false)]
        public bool PayStubs
        {
            get { return (Boolean)this["payStubs"]; }
            set { this["payStubs"] = value; }
        }

        
    }
}
