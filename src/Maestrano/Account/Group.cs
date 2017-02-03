using Maestrano.Configuration;
using Maestrano.Helpers;
using Newtonsoft.Json;
using System;

namespace Maestrano.Account
{
    public class Group : MnoObject
    {
        /// <summary>
        /// The current status of the group
        /// </summary>
        [JsonProperty("status")]
        public String Status { get; set; }

        /// <summary>
        /// The group name
        /// </summary>
        [JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("has_credit_card")]
        public Boolean HasCreditCard { get; set; }

        /// <summary>
        /// When the group free trial will be finishing on Maestrano. You may optionally consider this date for your own free trial (optional)
        /// </summary>
        [JsonProperty("free_trial_end_at")]
        public DateTime FreeTrialEndAt { get; set; }

        /// <summary>
        /// The principal email address for this group (admin email address)
        /// </summary>
        [JsonProperty("email")]
        public String Email { get; set; }

        /// <summary>
        /// The currency used by this Group in http://en.wikipedia.org/wiki/ISO_4217#Active_codes ISO 4217 format (3 letter code)
        /// </summary>
        [JsonProperty("currency")]
        public String Currency { get; set; }

        /// <summary>
        /// The group timezone
        /// </summary>
        [JsonProperty("timezone")]
        public String TimezoneAsString { get; set; }

        /// <summary>
        /// The country of the group in http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2 ISO 3166-1 alpha-2 format (2 letter code). E.g: 'US' for USA, 'AU' for Australia
        /// </summary>
        [JsonProperty("country")]
        public String Country { get; set; }

        /// <summary>
        /// The city of the group
        /// </summary>
        [JsonProperty("city")]
        public String City { get; set; }

        /// <summary>
        /// The group timezone
        /// </summary>
        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneConverter.fromOlsonTz(this.TimezoneAsString); }
            private set { }
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
        /// Scope REST calls to specific configuration presets
        /// </summary>
        /// <param name="presetName">name of preset to use</param>
        /// <returns></returns>
        public static GroupRequestor With(Preset preset)
        {
            return new GroupRequestor(preset);
        }
    }
}
