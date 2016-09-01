using Maestrano.Net;

namespace Maestrano.Connec
{
    public class Client : JsonClient
    {
        private Client(string host, string path, string key, string secret) : base(host, path, key, secret)
        {
        }

        /// <summary>
        /// Return a Client for a specific preset
        /// New scoped clients can be initialized with: Client.New("group-id", "preset")
        /// </summary>
        /// <param name="presetName"></param>
        /// <returns></returns>
        public static Client New(string groupId, string presetName = "maestrano") {
            if (presetName == null)
            {
                presetName = "maestrano";
            }
            var preset = MnoHelper.With(presetName);
            var host = preset.Connec.Host;
            var path = preset.Connec.BasePath;
            var apiId = preset.Api.Id;
            var apiKey = preset.Api.Key;
            var connecScopedPath = path + "/" + groupId;
            return new Client(host, connecScopedPath, apiId, apiKey);
        }
    }
}
