using Maestrano.Api;
using Maestrano.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Account
{
    public class Group
    {
        [JsonProperty("id")]
        public String Id {get;set;}

        [JsonProperty("created_at")]
        public DateTime CreatedAt {get; set;}

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt {get; set;}

        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("has_credit_card")]
        public Boolean HasCreditCard { get; set; }

        [JsonProperty("free_trial_end_at")]
        public DateTime FreeTrialEndAt { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("currency")]
        public String Currency { get; set; }

        [JsonProperty("timezone")]
        public String TimezoneAsString { get; set; }

        [JsonProperty("country")]
        public String Country {get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneConverter.fromOlsonTz(this.TimezoneAsString); }
            private set {}
        }

        /// <summary>
        /// The Resource name
        /// </summary>
        /// <returns></returns>
        public static string IndexPath()
        {
            return "account/groups";
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
        /// Retrieve all Maestrano groups having access to your application
        /// </summary>
        /// <param name="filters">User attributes to filter on</param>
        /// <returns></returns>
        public static List<Group> All(NameValueCollection filters = null)
        {
            return MnoClient.All<Group>(IndexPath(), filters);
        }

        /// <summary>
        /// Retrieve a single Maestrano group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Group Retrieve(string groupId)
        {
            return MnoClient.Retrieve<Group>(ResourcePath(), groupId);
        }
    }
}
