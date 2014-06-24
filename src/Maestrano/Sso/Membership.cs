using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Maestrano.Sso
{
    public class Membership
    {
        public string UserUid { get; set; }
        public string GroupUid { get; set; }
        public string Role { get; set; }

        /// <summary>
        /// Constructor loading group attributes from a Saml.Response
        /// </summary>
        /// <param name="samlResponse"></param>
        public Membership(Saml.Response samlResponse)
        {
            NameValueCollection att = samlResponse.GetAttributes();
            UserUid = att["uid"];
            GroupUid = att["group_uid"];
            Role = att["group_role"];
        }

        /// <summary>
        /// Return a serializable dictionary describing the resource
        /// </summary>
        /// <returns></returns>
        public JObject ToHash()
        {
            return new JObject(
                new JProperty("provider", "maestrano"),
                new JProperty("group_uid", UserUid),
                new JProperty("user_uid", GroupUid),
                new JProperty("role", Role)
             );

        }
    }
}
