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
    public class User
    {
        [JsonProperty("id")]
        public String Id {get; set;}

        [JsonProperty("name")]
        public String FirstName { get; set; }

        [JsonProperty("surname")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("company_name")]
        public String CompanyName { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

        [JsonProperty("sso_session")]
        public String SsoSession { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The Resource name
        /// </summary>
        /// <returns></returns>
        public static string IndexPath()
        {
            return "account/users";
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
        /// Retrieve all Maestrano users having access to your application
        /// </summary>
        /// <param name="filters">User attributes to filter on</param>
        /// <returns></returns>
        public static List<User> All(NameValueCollection filters = null)
        {
            return MnoClient.All<User>(IndexPath(), filters);
        }

        /// <summary>
        /// Retrieve a single Maestrano user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static User Retrieve(string userId)
        {
            return MnoClient.Retrieve<User>(ResourcePath(), userId);
        }
    }
}
