using Maestrano.Configuration;
using Maestrano.Net;

namespace Maestrano.Connec
{
    public class Client : JsonClient
    {
        public Client(string host, string path, string key, string secret) : base(host, path, key, secret) { }

        public static Client New(Preset preset, string groupId)
        {
            var host = preset.Connec.Host;
            var path = preset.Connec.BasePath;
            var apiId = preset.Api.Id;
            var apiKey = preset.Api.Key;
            var connecScopedPath = path + "/" + groupId;
            return new Client(host, connecScopedPath, apiId, apiKey);
        }
    }
}
