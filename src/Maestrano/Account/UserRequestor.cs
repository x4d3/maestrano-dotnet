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
    public class UserRequestor:MnoClient<User>
    {
        public UserRequestor(Preset preset) : base(User.IndexPath(), User.ResourcePath(), preset)
        {
        }

        /// <summary>
        /// Check a user password. Useful if you have specific privileged actions
        /// requiring users to confirm their password.
        /// </summary>
        /// <param name="userIdOrEmail">user id or email address</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        public Boolean CheckPassword(string userIdOrEmail, string password)
        {
            var rgx = new Regex(@".*@.*\.(\w+)");
            var att = new NameValueCollection();
            att.Add("password", MnoEncryptor.encrypt(password, preset.Api.Key));

            // Check if we should match the password against the id or email
            if (rgx.IsMatch(userIdOrEmail)) {
                att.Add("email", userIdOrEmail);
            } else {
                att.Add("id", userIdOrEmail);
            }

            try
            {
                // Raise an error on failure
                Create(User.IndexPath() + "/authenticate", att);
                return true;
            } catch (ResourceException) {
                return false;
            }
        }
    }
}
