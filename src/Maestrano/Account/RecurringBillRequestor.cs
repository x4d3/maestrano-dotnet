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
    class RecurringBillRequestor
    {

        private string presetName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preset">Name of a preset</param>
        public RecurringBillRequestor(string presetName = "maestrano")
        {
            this.presetName = presetName;
        }

        public List<RecurringBill> All(NameValueCollection filters = null)
        {
            return MnoClient.All<RecurringBill>(RecurringBill.IndexPath(), filters, presetName);
        }

        public RecurringBill Retrieve(string billId)
        {
            return MnoClient.Retrieve<RecurringBill>(RecurringBill.ResourcePath(), billId, presetName);
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

            return MnoClient.Create<RecurringBill>(RecurringBill.IndexPath(), att, presetName);
        }
    }
}
