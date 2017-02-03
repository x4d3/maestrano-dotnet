using Maestrano.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Maestrano.Configuration
{
    public class WebhookConnecSubscriptions : Dictionary<String, Boolean>
    {
        public static WebhookConnecSubscriptions LoadFromJson(JObject obj)
        {
            var config = new WebhookConnecSubscriptions();
            foreach(var x in obj) {
                config[StringExtensions.ToCamelCase(x.Key)] = x.Value.Value<Boolean>();
            }
            return config;
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Accounts
        {
            get { return (Boolean)this["accounts"]; }
            set { this["accounts"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Company
        {
            get { return (Boolean)this["company"]; }
            set { this["company"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer invoices
        /// </summary>
        [ConfigurationProperty("invoices", DefaultValue = false, IsRequired = false)]
        public bool Invoices
        {
            get { return (Boolean)this["invoices"]; }
            set { this["invoices"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer sales Orders
        /// </summary>
        public bool SalesOrders
        {
            get { return (Boolean)this["salesOrders"]; }
            set { this["salesOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer purchase Orders
        /// </summary>
        public bool PurchaseOrders
        {
            get { return (Boolean)this["purchaseOrders"]; }
            set { this["purchaseOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer quotes
        /// </summary>
        public bool Quotes
        {
            get { return (Boolean)this["quotes"]; }
            set { this["quotes"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Payments
        {
            get { return (Boolean)this["payments"]; }
            set { this["payments"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Journals
        {
            get { return (Boolean)this["journals"]; }
            set { this["journals"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Items
        {
            get { return (Boolean)this["items"]; }
            set { this["items"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Organizations
        {
            get { return (Boolean)this["organizations"]; }
            set { this["organizations"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool People
        {
            get { return (Boolean)this["people"]; }
            set { this["people"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
    
        public bool Projects
        {
            get { return (Boolean)this["projects"]; }
            set { this["projects"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool TaxCodes
        {
            get { return (Boolean)this["taxCodes"]; }
            set { this["taxCodes"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool TaxRates
        {
            get { return (Boolean)this["taxRates"]; }
            set { this["taxRates"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Events
        {
            get { return (Boolean)this["events"]; }
            set { this["events"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Venues
        {
            get { return (Boolean)this["venues"]; }
            set { this["venues"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool EventOrders
        {
            get { return (Boolean)this["eventOrders"]; }
            set { this["eventOrders"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool WorkLocations
        {
            get { return (Boolean)this["workLocations"]; }
            set { this["workLocations"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool PayItems
        {
            get { return (Boolean)this["payItems"]; }
            set { this["payItems"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool Employees
        {
            get { return (Boolean)this["employees"]; }
            set { this["employees"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool PaySchedules
        {
            get { return (Boolean)this["paySchedules"]; }
            set { this["paySchedules"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool TimeSheets
        {
            get { return (Boolean)this["timeSheets"]; }
            set { this["timeSheets"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool TimeActivities
        {
            get { return (Boolean)this["timeActivities"]; }
            set { this["timeActivities"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool PayRuns
        {
            get { return (Boolean)this["payRuns"]; }
            set { this["payRuns"] = value; }
        }

        /// <summary>
        /// Whether to receive notifications related to the customer company
        /// </summary>
        public bool PayStubs
        {
            get { return (Boolean)this["payStubs"]; }
            set { this["payStubs"] = value; }
        }
    }
}
