using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using RestSharp;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Maestrano.Helpers;

namespace Maestrano.Api
{
    public static class MnoClient
    {
        private static Dictionary<string, RestClient> clientDict;

        static MnoClient()
        {
            clientDict = new Dictionary<string, RestClient>();
        }

        private static RestClient Client(string presetName = "maestrano")
        {
            if (!clientDict.ContainsKey(presetName))
            {
                var client = new RestClient();

                // silverlight friendly way to get current version
                var assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = new AssemblyName(assembly.FullName);
                var version = assemblyName.Version;

                client = new RestClient();
                client.UserAgent = "maestrano-dotnet/" + version;
                client.Authenticator = new HttpBasicAuthenticator(MnoHelper.With(presetName).Api.Id, MnoHelper.With(presetName).Api.Key);
                client.BaseUrl = String.Format("{0}{1}", MnoHelper.With(presetName).Api.Host, MnoHelper.With(presetName).Api.Base);
                clientDict.Add(presetName, client);
            }

            return clientDict[presetName];
        }

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public static T ProjectSingleObject<T>(RestRequest request, string presetName = "maestrano")
        {
            request.OnBeforeDeserialization = (resp) =>
            {
                // for individual resources when there's an error to make
                // sure that RestException props are populated
                if (((int)resp.StatusCode) >= 400)
                    request.RootElement = "";
            };

            var response = Client(presetName).Execute(request);
            var respObj = JsonConvert.DeserializeObject<MnoObject<T>>(response.Content);
            respObj.ThrowIfErrors();
            respObj.AssignPreset(presetName);

            return respObj.Data;
        }

        /// <summary>
        /// Execute a manual REST request
        /// </summary>
        /// <typeparam name="T">The type of object to create and populate with the returned data.</typeparam>
        /// <param name="request">The RestRequest to execute (will use client credentials)</param>
        public static List<T> ProcessList<T>(RestRequest request, string presetName = "maestrano")
        {
            request.OnBeforeDeserialization = (resp) =>
            {
                // for individual resources when there's an error to make
                // sure that RestException props are populated
                //if (((int)resp.StatusCode) >= 400)
                //    request.RootElement = "";
            };

            var response = Client(presetName).Execute(request);
            var respObj = JsonConvert.DeserializeObject<MnoCollection<T>>(response.Content);
            respObj.ThrowIfErrors();
            respObj.AssignPreset(presetName);

            return respObj.Data;
        }

        public static List<T> All<T>(string path, NameValueCollection filters = null, string presetName = "maestrano")
        {
            var request = new RestRequest();
            request.Resource = path;
            request.Method = Method.GET;

            // Add query parameters
            if (filters != null)
                foreach (String k in filters.AllKeys)
                    request.AddParameter(k, filters[k]);

            return MnoClient.ProcessList<T>(request, presetName);
        }

        public static T Retrieve<T>(string path, string resourceId, string presetName = "maestrano")
        {
            var request = new RestRequest();
            request.Resource = path;
            request.Method = Method.GET;
            request.AddUrlSegment("id", resourceId);

            return MnoClient.ProjectSingleObject<T>(request, presetName);
        }

        public static T Create<T>(string path, NameValueCollection parameters, string presetName = "maestrano")
        {
            var request = new RestRequest();
            request.Resource = path;
            request.Method = Method.POST;

            foreach (var k in parameters.AllKeys)
                request.AddParameter(StringExtensions.ToSnakeCase(k), parameters[k]);

            return MnoClient.ProjectSingleObject<T>(request,presetName);
        }

        public static T Delete<T>(string path, string resourceId, string presetName = "maestrano")
        {
            var request = new RestRequest();
            request.Resource = path;
            request.Method = Method.DELETE;
            request.AddUrlSegment("id", resourceId);

            return MnoClient.ProjectSingleObject<T>(request,presetName);
        }
    }
}
