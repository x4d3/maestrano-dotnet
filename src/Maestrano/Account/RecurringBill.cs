using Maestrano.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Account
{
    public class RecurringBill
    {
        public string PresetName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("units")]
        public decimal? Units { get; set; }

        [JsonProperty("period")]
        public String Period { get; set; }

        [JsonProperty("frequency")]
        public Int16? Frequency { get; set; }

        [JsonProperty("cycles")]
        public Int16? Cycles { get; set; }

        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("initial_cents")]
        public Int32? InitialCents { get; set; }

        // Mandatory for creation

        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        [JsonProperty("price_cents")]
        public Int32 PriceCents { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The Resource name
        /// </summary>
        /// <returns></returns>
        public static string IndexPath()
        {
            return "account/recurring_bills";
        }

        /// <summary>
        /// The Single Resource name
        /// </summary>
        /// <returns></returns>
        public static string ResourcePath()
        {
            return IndexPath() + "/{id}";
        }

        /// <summary>
        /// Scope REST calls to specific configuration presets
        /// </summary>
        /// <param name="presetName">name of preset to use</param>
        /// <returns></returns>
        public static RecurringBillRequestor With(string presetName = "maestrano")
        {
            return new RecurringBillRequestor(presetName);
        }

        public static List<RecurringBill> All(NameValueCollection filters = null)
        {
            return (new RecurringBillRequestor()).All(filters);
        }

        public static RecurringBill Retrieve(string billId)
        {
            return (new RecurringBillRequestor()).Retrieve(billId);
        }

        public static RecurringBill Create(String groupId, Int32 priceCents, String description, String currency = "AUD", Int32 initialCents = 0, Int16? frequency = null, Int16? cycles = null, DateTime? startDate = null)
        {
            return (new RecurringBillRequestor()).Create(
                groupId: groupId,
                priceCents: priceCents,
                description: description,
                currency: currency,
                initialCents: initialCents,
                frequency: frequency,
                cycles: cycles,
                startDate: startDate
                );
        }

        public Boolean Cancel()
        {
            if (Id != null)
            {
                RecurringBill respBill = MnoClient.Delete<RecurringBill>(ResourcePath(), Id);
                Status = respBill.Status;
                UpdatedAt = respBill.UpdatedAt;
                return (Status.Equals("cancelled"));
            }

            return false;
        }
    }
}
