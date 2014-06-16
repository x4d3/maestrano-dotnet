using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class WebhookAccount
    {
        // Application REST endpoint for groups
        private string _groupspath;
        public string GroupsPath 
        {
            get
            {
                if (string.IsNullOrEmpty(_groupspath))
                {
                    return "/maestrano/account/groups/:id";
                }
                return _groupspath;
            }

            set { _groupspath = value; }
        }

        // Application REST endpoint for group > users
        private string _groupuserspath;
        public string GroupUsersPath 
        {
            get
            {
                if (string.IsNullOrEmpty(_groupuserspath))
                {
                    return "/maestrano/account/groups/:group_id/users/:id";
                }
                return _groupuserspath;
            }

            set { _groupuserspath = value; }
        }
    }
}
