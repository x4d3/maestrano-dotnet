using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Maestrano
{
    public static class Mno
    {
        // VERSION
        public static string Version { get { return "0.2.0"; } }

        
        public static Configuration.Sso Sso { get; private set; }
        public static Configuration.App App { get; private set; }
        public static Configuration.Api Api { get; private set; }
        public static Configuration.Webhook Webhook { get; private set; }

        static Mno()
        {
            App = Configuration.App.Load();
            Api = Configuration.Api.Load();
            Webhook = Configuration.Webhook.Load();
            Sso = Configuration.Sso.Load();
        }

        /// <summary>
        /// App environment: 'test' or 'production'
        /// </summary>
        public static string Environment {
            get { return Mno.App.Environment; }
            set { Mno.App.Environment = value; }
        }

        /// <summary>
        /// Check that ID and Key passed in argument match
        /// the ones configured at the API level.
        /// Used for remote authentication from Maestrano.
        /// </summary>
        /// <param name="apiId">An application ID</param>
        /// <param name="apiKey">An API Key</param>
        /// <returns>true if the authentication is successful, false otherwise</returns>
        public static bool Authenticate(string apiId, string apiKey)
        {
            return Api.Id == apiId && Api.Key == apiKey;
        }

        /// <summary>
        /// Take a 
        /// </summary>
        /// <returns></returns>
        public static bool Authenticate(System.Web.HttpRequest request)
        {
            bool authenticated = false;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                        StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    var credentials = authHeaderVal.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    credentials = encoding.GetString(Convert.FromBase64String(credentials));

                    int separator = credentials.IndexOf(':');
                    string apiId = credentials.Substring(0, separator);
                    string apiKey = credentials.Substring(separator + 1);
                    authenticated = Authenticate(apiId,apiKey);
                }
            }

            return authenticated;
        }

        /// <summary>
        /// Take a user uid (either real like 'usr-1' or virtual like
        /// 'usr-1.cld-3') and return the real uid part
        /// </summary>
        /// <param name="userUid">A real or virtual uid</param>
        /// <returns>Real user uid</returns>
        public static string UnmaskUser(string userUid)
        {
            string[] words = userUid.Split('.');
            if (words.Length > 0)
            {
                return words.First();
            }
            return userUid;
        }

        /// <summary>
        /// Take a user uid (either real or virtual) and a group uid
        /// and return the user uid that should be used within the app
        /// based on the Sso.CreationMode parameter
        /// </summary>
        /// <param name="userUid">a real or virtual user uid</param>
        /// <param name="groupUid">a group uid</param>
        /// <returns>
        /// The real user uid (usr-1) if Sso.CreationMode is set to "real"
        /// The virtual user uid (user-1.cld-2) if Sso.CreationMode is set to "virtual"
        /// </returns>
        public static string MaskUser(string userUid, string groupUid)
        {
            string sanitizedUserUid = UnmaskUser(userUid);

            if (Sso.CreationMode == "virtual")
            {
                return sanitizedUserUid + '.' + groupUid;
            }
            else
            {
                return sanitizedUserUid;
            }
        }

        /// <summary>
        /// Return a json serializable object describing the current 
        /// Maestrano configuration. The metadata will be fetched remotely
        /// by Maestrano. Note that the metadata exclude any info
        /// including an API Key.
        /// </summary>
        /// <returns>JObject which can be converted to JSON using ToString()</returns>
        public static JObject ToMetadata()
        {
            JObject metadata = new JObject(
                new JProperty("environment", Mno.Environment),
                new JProperty("app", new JObject(new JProperty("host", Mno.App.Host))),
                new JProperty("api", new JObject(
                    new JProperty("id", Mno.Api.Id),
                    new JProperty("lang", Mno.Api.Lang),
                    new JProperty("version", Mno.Api.Version),
                    new JProperty("lang_version", Mno.Api.LangVersion))),
                new JProperty("sso", new JObject(
                    new JProperty("enabled", Mno.Sso.Enabled),
                    new JProperty("creation_mode", Mno.Sso.CreationMode),
                    new JProperty("init_path", Mno.Sso.InitPath),
                    new JProperty("consume_path", Mno.Sso.ConsumePath),
                    new JProperty("idm", Mno.Sso.Idm),
                    new JProperty("idp", Mno.Sso.Idp),
                    new JProperty("name_id_format", Mno.Sso.NameIdFormat),
                    new JProperty("x509_fingerprint", Mno.Sso.X509Fingerprint),
                    new JProperty("x509_certificate", Mno.Sso.X509Certificate))),
                new JProperty("webhook", new JObject(
                    new JProperty("account", new JObject(
                        new JProperty("groups_path", Mno.Webhook.Account.GroupsPath),
                        new JProperty("group_users_path", Mno.Webhook.Account.GroupUsersPath)
                        ))))
            );

            return metadata;
        }
    }
}
