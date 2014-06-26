using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using RestSharp;

namespace Maestrano.Api
{
    class MnoClient
    {
        public partial class MnoClient
        {
            public string ApiVersion { get; set; }
            public string ApiEndpoint { get; set; }
            public string ApiKey { get; private set; }

            private RestClient _client;

            public MnoClient(string apiKey)
            {
                ApiVersion = "v1";
                ApiEndpoint = "https://maestrano.com/api";
                ApiKey = apiKey;

                // silverlight friendly way to get current version
                var assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = new AssemblyName(assembly.FullName);
                var version = assemblyName.Version;

                _client = new RestClient();
                _client.UserAgent = "maestrano-dotnet/" + version;
                _client.Authenticator = new MnoAuthenticator(apiKey);
                _client.BaseUrl = String.Format("{0}{1}", ApiEndpoint, ApiVersion);
            }

            /// <summary>
            /// Execute a manual REST request
            /// </summary>
            /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
            /// <param name="request">The RestRequest to execute (will use client credentials)</param>
            public MnoObject ExecuteObject(RestRequest request)
            {
                request.OnBeforeDeserialization = (resp) =>
                {
                    // for individual resources when there's an error to make
                    // sure that RestException props are populated
                    if (((int)resp.StatusCode) >= 400)
                        request.RootElement = "";
                };

                var response = _client.Execute(request);
                var json = Deserialize(response.Content);
                var obj = new MnoObject();
                obj.SetModel(json);

                return obj;
            }

            /// <summary>
            /// Execute a manual REST request
            /// </summary>
            /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
            /// <param name="request">The RestRequest to execute (will use client credentials)</param>
            public MnoArray ExecuteArray(RestRequest request)
            {
                request.OnBeforeDeserialization = (resp) =>
                {
                    // for individual resources when there's an error to make
                    // sure that RestException props are populated
                    if (((int)resp.StatusCode) >= 400)
                        request.RootElement = "";
                };

                var response = _client.Execute(request);
                var json = Deserialize(response.Content);
                var obj = new MnoArray();
                obj.SetModel(json);

                return obj;
            }

            private IDictionary<string, object> Deserialize(string input)
            {
                if (String.IsNullOrEmpty(input))
                    return null;

                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<IDictionary<string, object>>(input);
            }
        }
    }
}
