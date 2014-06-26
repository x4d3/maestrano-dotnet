using Maestrano.Api;
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
        public string Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String Status { get; set; }
        public decimal? Units { get; set; }
        public String Period { get; set; }
        public Int16? Frequency { get; set; }
        public Int16? Cycles { get; set; }
        public DateTime? StartDate { get; set; }

        // Mandatory for creation
        public string GroupId { get; set; }
        public Int32 PriceCents { get; set; }
        public string Currency { get; set; }
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

        public static List<RecurringBill> All(NameValueCollection filters = null)
        {
            return MnoClient.All<RecurringBill>(IndexPath(), filters);
        }

        public static RecurringBill Retrieve(string billId)
        {
            return MnoClient.Retrieve<RecurringBill>(ResourcePath(), billId);
        }

        public static RecurringBill Create(String groupId, Int32 priceCents, String description, String currency = "AUD", Int16? frequency = null, Int16? cycles = null, DateTime? startDate = null)
        {
            var att = new NameValueCollection();
            att.Add("groupId", groupId);
            att.Add("priceCents", priceCents.ToString());
            att.Add("description", description);
            att.Add("currency", currency);
            if (frequency.HasValue)
                att.Add("frequency", frequency.Value.ToString());
            if (cycles.HasValue)
                att.Add("cycles", cycles.Value.ToString());
            if (startDate.HasValue)
                att.Add("startDate", startDate.Value.ToString("s"));

            return MnoClient.Create<RecurringBill>(IndexPath(), att);
        }

        public Boolean Cancel()
        {
            if (Id != null)
            {
                RecurringBill respBill = MnoClient.Delete<RecurringBill>(ResourcePath(), Id);
                Status = respBill.Status;
                return (Status.Equals("cancelled"));
            }

            return false;
        }
    }
}
