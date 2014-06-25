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

        /// <summary>
        /// Contructor retrieving maestrano session from
        /// http session
        /// </summary>
        /// <param name="httpSessionObj"></param>
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

        /// <summary>
        /// Contructor retrieving maestrano session from user
        /// </summary>
        /// <param name="httpSessionObj"></param>
        /// <param name="user"></param>
        public Session(HttpSessionState httpSessionObj, User user)
        {
            HttpSession = httpSessionObj;

            if (user != null)
            {
                Uid = user.Uid;
                GroupUid = user.GroupUid;
                SessionToken = user.SsoSession;
                Recheck = user.SsoSessionRecheck;
            }
        }

        /// <summary>
        /// Returns whether the session needs to be checked
        /// remotely from maestrano or not
        /// </summary>
        /// <returns></returns>
        public Boolean isRemoteCheckRequired()
        {
            if (Uid != null && SessionToken != null && Recheck != null)
            {
                return (Recheck.CompareTo(DateTime.UtcNow) <= 0);
            }
            return true;
        }
    }
}
