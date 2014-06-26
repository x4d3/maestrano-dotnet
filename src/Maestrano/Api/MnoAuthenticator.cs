using System;
using System.Linq;
using RestSharp;
using System.Net;

namespace Maestrano.Api
{
    public class MnoAuthenticator : IAuthenticator
    {
        private readonly string _apiKey;
        private readonly string _apiId;

        public MnoAuthenticator(string apiId, string apiKey)
        {
            _apiId = apiId;
            _apiKey = apiKey;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.Credentials = new NetworkCredential(_apiId,_apiKey);
        }
    }
}
