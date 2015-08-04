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
    class GroupRequestor
    {
        private string presetName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preset">Name of a preset</param>
        public GroupRequestor(string presetName = "maestrano")
        {
            this.presetName = presetName;
        }

        /// <summary>
        /// Retrieve all Maestrano groups having access to your application
        /// </summary>
        /// <param name="filters">User attributes to filter on</param>
        /// <returns></returns>
        public List<Group> All(NameValueCollection filters = null)
        {
            return MnoClient.All<Group>(Group.IndexPath(), filters, presetName);
        }

        /// <summary>
        /// Retrieve a single Maestrano group by id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Group Retrieve(string groupId)
        {
            return MnoClient.Retrieve<Group>(Group.ResourcePath(), groupId, presetName);
        }
    }
}
