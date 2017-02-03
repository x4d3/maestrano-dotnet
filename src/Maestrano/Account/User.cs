using Maestrano.Api;
using Maestrano.Configuration;
using Maestrano.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maestrano.Account
{
    public class User : MnoObject
    {
        /// <summary>
        /// The user first name
        /// </summary>
        [JsonProperty("name")]
        public String FirstName { get; set; }

        /// <summary>
        /// The user Last name
        /// </summary>
        [JsonProperty("surname")]
        public String LastName { get; set; }

        /// <summary>
        ///The user email address
        /// </summary>
        [JsonProperty("email")]
        public String Email { get; set; }

        /// <summary>
        ///The user company name as it was entered when they signed up. Nothing related to the user group name.
        /// </summary>
        [JsonProperty("company_name")]
        public String CompanyName { get; set; }

        /// <summary>
        ///The country of the user in http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2 ISO 3166-1 alpha-2 format</a> (2 letter code). E.g: 'US' for USA, 'AU' for Australia.
        /// </summary>
        [JsonProperty("country")]
        public String Country { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("sso_session")]
        public String SsoSession { get; set; }

        /// <summary>
        /// The Resource name
        /// </summary>
        public static string IndexPath()
        {
            return "account/users";
        }

        /// <summary>
        /// The Single Resource name
        /// </summary>
        public static string ResourcePath()
        {
            return IndexPath() + "/{id}";
        }

        public static UserRequestor With(Preset preset)
        {
            return new UserRequestor(preset);
        }

    }
}
