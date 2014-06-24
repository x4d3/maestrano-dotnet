using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Maestrano.Sso
{
    public class Group
    {
        public string Uid { get; set; }
        public DateTime FreeTrialEndAt { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }

        /// <summary>
        /// Constructor loading group attributes from a Saml.Response
        /// </summary>
        /// <param name="samlResponse"></param>
        public Group(Saml.Response samlResponse)
        {
            NameValueCollection att = samlResponse.GetAttributes();
            Uid = att["group_uid"];
            FreeTrialEndAt = DateTime.Parse(att["group_end_free_trial"]);
            Country = att["country"];
            CompanyName = att["company_name"];
        }

        /// <summary>
        /// Return a serializable dictionary describing the resource
        /// </summary>
        /// <returns></returns>
        public JObject ToHash()
        {
            return new JObject(
                new JProperty("provider", "maestrano"),
                new JProperty("uid", Uid),
                new JProperty("info", new JObject(
                    new JProperty("free_trial_end_at", FreeTrialEndAt),
                    new JProperty("company_name", CompanyName),
                    new JProperty("country", Country))),
                new JProperty("extra",null)
             );

        }
    }
}
