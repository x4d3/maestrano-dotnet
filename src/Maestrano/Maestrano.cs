using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Maestrano.Configuration;

namespace Maestrano
{
    public static class MnoHelper
    {
        // VERSION
        public static string Version { get { return "0.14.0"; } }

        private static Dictionary<string,Preset> presetDict;
        private static Preset defaultPreset;
        public static Configuration.Sso Sso { get; private set; }
        public static Configuration.App App { get; private set; }
        public static Configuration.Api Api { get; private set; }
        public static Configuration.Connec Connec { get; private set; }
        public static Configuration.Webhook Webhook { get; private set; }

        static MnoHelper()
        {
            // Initialize preset list
            presetDict = new Dictionary<string, Preset>();

            // Load "maestrano" preset by default
            defaultPreset = new Preset("maestrano");
            presetDict.Add("maestrano", defaultPreset);

            // Emulate old configuration behaviour for backward 
            // compatibility
            App = defaultPreset.App;
            Api = defaultPreset.Api;
            Connec = defaultPreset.Connec;
            Webhook = defaultPreset.Webhook;
            Sso = defaultPreset.Sso;
        }

        /// <summary>
        /// Scope the MnoHelper to a specific configuration preset
        /// </summary>
        public static Preset With(string presetName = "maestrano") {
            if (presetName == null)
            {
                presetName = "maestrano";
            }
            
            if (!presetDict.ContainsKey(presetName))
            {
                presetDict.Add(presetName, new Preset(presetName));
            }

            return presetDict[presetName];
        }

        /// <summary>
        /// App environment: 'test' or 'production'
        /// </summary>
        public static string Environment {
            get { return defaultPreset.App.Environment; }
            set { defaultPreset.App.Environment = value; }
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
            return defaultPreset.Authenticate(apiId,apiKey);
        }

        /// <summary>
        /// Authenticate a request from Maestrano using HTTP Basic Authentication
        /// </summary>
        /// <param name="request">An HttpRequest object</param>
        /// <returns>true if the authentication is successful, false otherwise</returns>
        public static bool Authenticate(System.Web.HttpRequest request)
        {
            return defaultPreset.Authenticate(request);
        }

        /// <summary>
        /// Take a user uid (either real like 'usr-1' or virtual like
        /// 'usr-1.cld-3') and return the real uid part
        /// </summary>
        /// <param name="userUid">A real or virtual uid</param>
        /// <returns>Real user uid</returns>
        public static string UnmaskUser(string userUid)
        {
            return defaultPreset.UnmaskUser(userUid);
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
            return defaultPreset.MaskUser(userUid, groupUid);
        }

        /// <summary>
        /// Return whether the environment is production like (production or production sandbox)
        /// </summary>
        /// <returns>true for production and production-sandbox</returns>
        public static Boolean isProduction()
        {
            return defaultPreset.isProduction();
        }

        /// <summary>
        /// Return whether the environment is production like (production or production sandbox)
        /// </summary>
        /// <returns>true for production and production-sandbox</returns>
        public static Boolean isDevelopment()
        {
            return defaultPreset.isDevelopment();
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
            return defaultPreset.ToMetadata();
        }
    }
}
