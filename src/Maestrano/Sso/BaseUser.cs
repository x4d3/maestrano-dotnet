using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Sso
{

    public class BaseUser
    {
        public string SsoSession { get; set; }
        public DateTime SsoSessionRecheck { get; set; }
        public string GroupUid { get; set; }
        public string GroupRole { get; set; }
        public string Uid { get; set; }
        public string VirtualUid { get; set; }
        public string Email { get; set; }
        public string VirtualEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }

        public BaseUser(Saml.Response samlResponse)
        {
            NameValueCollection att = samlResponse.GetAttributes();
            SsoSession = att["mno_session"];
            SsoSessionRecheck = DateTime.Parse(att["mno_session_recheck"]);
            GroupUid = att["group_uid"];
            GroupRole = att["group_role"];
            Uid = att["uid"];
            VirtualUid = att["virtual_uid"];
            Email = att["email"];
            VirtualEmail = att["virtual_email"];
            FirstName = att["name"];
            LastName = att["surname"];
            Country = att["country"];
            CompanyName = att["company_name"];

        }
    }
}
