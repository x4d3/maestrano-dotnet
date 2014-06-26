using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class WebhookAccount : ConfigurationSection
    {
        /// <summary>
        /// Load WebhooAccount configuration into a WebhooAccount configuration object
        /// </summary>
        /// <returns>A WebhooAccount configuration object</returns>
        public static WebhookAccount Load()
        {
            return ConfigurationManager.GetSection("maestrano/webhook/account") as WebhookAccount;
        }

        /// <summary>
        /// Return False (object not read only)
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }

        /// <summary>
        /// Application REST endpoint for groups
        /// </summary>
        [ConfigurationProperty("groupsPath", DefaultValue = "/maestrano/account/groups/:id", IsRequired = false)]
        public String GroupsPath
        {
            get { return (String)this["groupsPath"]; }
            set { this["groupsPath"] = value; }
        }

        /// <summary>
        /// Application REST endpoint for group > users
        /// </summary>
        [ConfigurationProperty("groupUsersPath", DefaultValue = "/maestrano/account/groups/:group_id/users/:id", IsRequired = false)]
        public String GroupUsersPath
        {
            get { return (String)this["groupUsersPath"]; }
            set { this["groupUsersPath"] = value; }
        }
    }
}
