using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Maestrano
{
    public static class Maestrano
    {
        // VERSION
        public static string Version { get { return "0.1.0"; } }

        public static string Environment { get; set; }
        public static Configuration.Sso Sso { get; private set; }
        public static Configuration.App App { get; private set; }
        public static Configuration.Api Api { get; private set; }
        public static Configuration.Webhook Webhook { get; private set; }

        static Maestrano()
        {
            App = new Configuration.App();
            Api = new Configuration.Api();
            Webhook = new Configuration.Webhook();
            Sso = new Configuration.Sso();
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
                new JProperty("environment", Maestrano.Environment),
                new JProperty("app", new JObject(new JProperty("host", Maestrano.App.Host))),
                new JProperty("api", new JObject(
                    new JProperty("id", Maestrano.Api.Id),
                    new JProperty("lang", Maestrano.Api.Lang),
                    new JProperty("version", Maestrano.Api.Version),
                    new JProperty("lang_version", Maestrano.Api.LangVersion))),
                new JProperty("sso", new JObject(
                    new JProperty("enabled", Maestrano.Sso.Enabled),
                    new JProperty("creation_mode", Maestrano.Sso.CreationMode),
                    new JProperty("init_path", Maestrano.Sso.InitPath),
                    new JProperty("consume_path", Maestrano.Sso.ConsumePath),
                    new JProperty("idm", Maestrano.Sso.Idm),
                    new JProperty("idp", Maestrano.Sso.Idp),
                    new JProperty("name_id_format", Maestrano.Sso.NameIdFormat),
                    new JProperty("x509_fingerprint", Maestrano.Sso.X509Fingerprint),
                    new JProperty("x509_certificate", Maestrano.Sso.X509Certificate))),
                new JProperty("webhook", new JObject(
                    new JProperty("account", new JObject(
                        new JProperty("groups_path", Maestrano.Webhook.Account.GroupsPath),
                        new JProperty("group_users_path", Maestrano.Webhook.Account.GroupUsersPath)
                        ))))
            );

            return metadata;
        }
    }
}
