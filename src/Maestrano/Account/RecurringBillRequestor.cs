using Maestrano.Api;
using Maestrano.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Account
{
    public class RecurringBillRequestor:MnoClient<RecurringBill>
    {
        public RecurringBillRequestor(Preset preset) : base(RecurringBill.IndexPath(), RecurringBill.ResourcePath(), preset)
        {
        }

        public RecurringBill Create(String groupId, Int32 priceCents, String description, String currency = "AUD", Int32 initialCents = 0, Int16? frequency = null, Int16? cycles = null, DateTime? startDate = null)
        {
            var att = new NameValueCollection();
            att.Add("groupId", groupId);
            att.Add("priceCents", priceCents.ToString());
            att.Add("description", description);
            att.Add("currency", currency);
            att.Add("initialCents", initialCents.ToString());
            if (frequency.HasValue)
                att.Add("frequency", frequency.Value.ToString());
            if (cycles.HasValue)
                att.Add("cycles", cycles.Value.ToString());
            if (startDate.HasValue)
                att.Add("startDate", startDate.Value.ToString("s"));

            return Create(att);
        }

        /// <summary>
        /// Cancel the recurring Bill
        /// </summary>
        /// <returns>if the recurring Bill was cancelled</returns>
        public Boolean Cancel(String id)
        {
            if (id != null)
            {
                RecurringBill respBill = Delete(id);
                var Status = respBill.Status;
                return (Status.Equals("cancelled"));
            }
            return false;
        }
    }
}
