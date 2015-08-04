using Maestrano.Api;
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
    class UserRequestor
    {

        private string presetName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preset">Name of a preset</param>
        public UserRequestor(string presetName = "maestrano")
        {
            this.presetName = presetName;
        }

        /// <summary>
        /// Retrieve all Maestrano users having access to your application
        /// </summary>
        /// <param name="filters">User attributes to filter on</param>
        /// <returns></returns>
        public List<User> All(NameValueCollection filters = null)
        {
            return MnoClient.All<User>(User.IndexPath(), filters, presetName);
        }

        /// <summary>
        /// Retrieve a single Maestrano user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User Retrieve(string userId)
        {
            return MnoClient.Retrieve<User>(User.ResourcePath(), userId, presetName);
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
            att.Add("password", MnoEncryptor.encrypt(password,MnoHelper.With(presetName).Api.Key));

            // Check if we should match the password against the id or email
            if (rgx.IsMatch(userIdOrEmail)) {
                att.Add("email", userIdOrEmail);
            } else {
                att.Add("id", userIdOrEmail);
            }

            try
            {
                // Raise an error on failure
                MnoClient.Create<User>(User.IndexPath() + "/authenticate", att, presetName);
                return true;
            } catch (Maestrano.Api.ResourceException) {
                return false;
            }
        }
    }
}
