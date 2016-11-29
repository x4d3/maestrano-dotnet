using Newtonsoft.Json.Linq;

namespace Maestrano.Configuration
{
    public class Webhook
    {
        public WebhookAccount Account { get; private set; }
        public WebhookConnec Connec { get; private set; }

        /// <summary>
        /// Load Webhook configuration into a Webhook configuration object
        /// </summary>
        /// <returns>A Webhook configuration object</returns>
        public static Webhook Load(string preset = "maestrano")
        {
            var config = new Webhook();
            config.Account = WebhookAccount.Load(preset);
            config.Connec = WebhookConnec.Load(preset);
            return config;
        }

        /// <summary>
        /// Load Webhook into a Connec configuration object from a JObject 
        /// </summary>
        /// <returns>A Webhook configuration object</returns>
        public static Webhook LoadFromJson(string preset, JObject obj)
        {
            var config = new Webhook();
            config.Account = WebhookAccount.LoadFromJson(preset, obj["account"].Value<JObject>());
            config.Connec = WebhookConnec.LoadFromJson(preset, obj["connec"].Value<JObject>());
            return config;
        }

    }
}
