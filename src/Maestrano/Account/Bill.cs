using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maestrano.Api;
using System.Collections.Specialized;
using RestSharp;
using Newtonsoft.Json;

namespace Maestrano.Account
{

    public class Bill
    {
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

        [JsonProperty("period_started_at")]
        public DateTime? PeriodStartedAt { get; set; }

        [JsonProperty("period_ended_at")]
        public DateTime? PeriodEndedAt { get; set; }

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
            return "account/bills";
        }

        /// <summary>
        /// The Single Resource name
        /// </summary>
        /// <returns></returns>
        public static string ResourcePath()
        {
            return IndexPath() + "/{id}";
        }

        public static List<Bill> All(NameValueCollection filters = null)
        {
            return MnoClient.All<Bill>(IndexPath(), filters);
        }

        public static Bill Retrieve(string billId)
        {
            return MnoClient.Retrieve<Bill>(ResourcePath(), billId);
        }

        public static Bill Create(String groupId, Int32 priceCents, String description, String currency = "AUD", Decimal? units = null, DateTime? periodStartedAt = null, DateTime? periodEndedAt = null)
        {
            var att = new NameValueCollection();
            att.Add("groupId", groupId);
            att.Add("priceCents",priceCents.ToString());
            att.Add("description",description);
            att.Add("currency",currency);
            if (units.HasValue)
                att.Add("units", units.Value.ToString());
            if (periodStartedAt.HasValue)
                att.Add("periodStartedAt",periodStartedAt.Value.ToString("s"));
            if (periodEndedAt.HasValue)
                att.Add("periodEndedAt", periodEndedAt.Value.ToString("s"));

            return MnoClient.Create<Bill>(IndexPath(),att);
        }

        public Boolean Cancel()
        {
            if (Id != null)
            {
                Bill respBill = MnoClient.Delete<Bill>(ResourcePath(), Id);
                Status = respBill.Status;
                return (Status.Equals("cancelled"));
            }

            return false;
        }
    }
}
