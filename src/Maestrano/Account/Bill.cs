using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maestrano.Api;
using System.Collections.Specialized;
using RestSharp;

namespace Maestrano.Account
{

    public class Bill
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public String Status { get; set; }
        public decimal? Units { get; set; }
        public DateTime PeriodStartedAt { get; set; }
        public DateTime PeriodEndedAt { get; set; }

        // Mandatory for creation
        public string GroupUid { get; set; }
        public Int32 PriceCents { get; set; }
        public string Currency { get; set; }
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

        public static Bill Create(String groupUid,Int32 priceCents, String description, String currency = "AUD", Decimal? units = null, DateTime? periodStartedAt = null, DateTime? periodEndedAt = null){
            var att = new NameValueCollection();
            att.Add("groupUid",groupUid);
            att.Add("priceCents",priceCents.ToString());
            att.Add("description",description);
            att.Add("currency",currency);
            if (units != null)
                att.Add("units", units.Value.ToString());
            if (periodStartedAt != null)
                att.Add("periodStartedAt",periodStartedAt.Value.ToString("s"));
            if (periodEndedAt != null)
                att.Add("periodEndedAt", periodEndedAt.Value.ToString("s"));

            return MnoClient.Create<Bill>(IndexPath(),att);
        }
    }
}
