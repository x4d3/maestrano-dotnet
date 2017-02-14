using Newtonsoft.Json.Linq;

namespace Maestrano.Configuration
{
    public class Webhook
    {
        public WebhookAccount Account { get; private set; }
        public WebhookConnec Connec { get; private set; }

        /// <summary>
        /// Load Webhook into a Connec configuration object from a JObject 
        /// </summary>
        /// <returns>A Webhook configuration object</returns>
        public static Webhook LoadFromJson(JObject obj)
        {
            var config = new Webhook();
            config.Account = WebhookAccount.LoadFromJson(obj["account"].Value<JObject>());
            config.Connec = WebhookConnec.LoadFromJson(obj["connec"].Value<JObject>());
            return config;
        }
    }
}
