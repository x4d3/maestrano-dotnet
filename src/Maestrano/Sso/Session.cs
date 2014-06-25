using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;

namespace Maestrano.Sso
{
    public class Session
    {

        public string Uid { get; set; }
        public string GroupUid { get; set; }
        public DateTime Recheck { get; set; }
        public string SessionToken { get; set; }
        public HttpSessionState HttpSession { get; set; }

        public Session(HttpSessionState httpSessionObj)
        {
            HttpSession = httpSessionObj;

            if (HttpSession != null && HttpSession["maestrano"] != null)
            {
                var enc = System.Text.Encoding.UTF8;
                JObject sessionObject = new JObject();
                try {
                    string decryptedMnoSession = enc.GetString(Convert.FromBase64String(HttpSession["maestrano"].ToString()));
                    sessionObject = JObject.Parse(decryptedMnoSession);
                }
                catch (Exception){ }
                
                // Assign attributes
                Uid = sessionObject.Value<String>("uid");
                GroupUid = sessionObject.Value<String>("group_uid");
                SessionToken = sessionObject.Value<String>("token");

                string recheckValue = sessionObject.Value<String>("recheck");
                if (recheckValue != null && recheckValue.Length > 0) {
                    try {
                    Recheck = DateTime.Parse(recheckValue);
                    }
                    catch (Exception){ }
                }
            }
        }
    }
}
