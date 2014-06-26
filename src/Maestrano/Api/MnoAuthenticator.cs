using System;
using System.Linq;
using RestSharp;
using System.Net;

namespace Maestrano.Api
{
    public class MnoAuthenticator : IAuthenticator
    {
        private readonly string _apiKey;

        public MnoAuthenticator(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.Credentials = new NetworkCredential(_apiKey, "");
        }
    }
}
