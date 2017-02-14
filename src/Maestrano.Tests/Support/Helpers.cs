using System;
using NUnit.Framework;
using System.Text;
using System.IO;
using Maestrano.Saml;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Moq;
using Maestrano.Configuration;

namespace Maestrano.Tests
{
    public class Helpers
    {

        public const string CERTIFICATE = "-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAPFpcH2rW0pyMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyNDEw\nWhcNMzMxMjMwMDUyNDEwWjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD3feNNn2xfEz5/\nQvkBIu2keh9NNhobpre8U4r1qC7h7OeInTldmxGL4cLHw4ZAqKbJVrlFWqNevM5V\nZBkDe4mjuVkK6rYK1ZK7eVk59BicRksVKRmdhXbANk/C5sESUsQv1wLZyrF5Iq8m\na9Oy4oYrIsEF2uHzCouTKM5n+O4DkwIDAQABo4HuMIHrMB0GA1UdDgQWBBSd/X0L\n/Pq+ZkHvItMtLnxMCAMdhjCBuwYDVR0jBIGzMIGwgBSd/X0L/Pq+ZkHvItMtLnxM\nCAMdhqGBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA8WlwfatbSnIwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQDE\nhe/18oRh8EqIhOl0bPk6BG49AkjhZZezrRJkCFp4dZxaBjwZTddwo8O5KHwkFGdy\nyLiPV326dtvXoKa9RFJvoJiSTQLEn5mO1NzWYnBMLtrDWojOe6Ltvn3x0HVo/iHh\nJShjAn6ZYX43Tjl1YXDd1H9O+7/VgEWAQQ32v8p5lA==\n-----END CERTIFICATE-----";


        private static readonly Encoding WINDOWS_ENCODING = Encoding.GetEncoding(1252);
        public static string CurrentMnoSession;

        public static void destroyMnoSession()
        {
          CurrentMnoSession = null;
        }

        // Used to build HttpSession
        public static void injectMnoSession()
        {
            injectMnoSession(DateTime.Parse("2014-06-22T01:00:00Z").ToUniversalTime());
        }

        public static void injectMnoSession(DateTime datetime)
        {
            JObject mnoContent = new JObject(
                    new JProperty("uid", "usr-1"),
                    new JProperty("session", "sessiontoken"),
                    new JProperty("group_uid", "cld-1"),
                    new JProperty("session_recheck", datetime.ToString("s"))
                );

            var enc = System.Text.Encoding.UTF8;
            CurrentMnoSession = Convert.ToBase64String(enc.GetBytes(mnoContent.ToString()));
        }

        public static HttpContextBase injectMnoSession(HttpContextBase context, DateTime datetime)
        {
            var httpSession = context.Session;
            JObject mnoContent = new JObject(
                    new JProperty("uid", "usr-1"),
                    new JProperty("session", "sessiontoken"),
                    new JProperty("group_uid", "cld-1"),
                    new JProperty("session_recheck", datetime.ToString("s"))
                );

            CurrentMnoSession = Convert.ToBase64String(Encoding.UTF8.GetBytes(mnoContent.ToString()));

            return context;
        }

        /// <summary>
        /// Read a reponse from the support/saml folder
        /// </summary>
        /// <param name="responseName"></param>
        /// <returns></returns>
        public static string ReadSamlSupportFiles(string responseName)
        {
            // Build path
            string responseFolder = "../../Support/Saml";
            string responsePath = responseFolder + "/" + responseName;

            using (StreamReader reader = new StreamReader(responsePath, WINDOWS_ENCODING))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Create a HttpContext with session
        /// </summary>
        /// <returns></returns>
        public static HttpContextBase FakeHttpContext(Preset preset)
        {
            var marketplace = preset.Marketplace;
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(c => c[marketplace]).Returns(CurrentMnoSession);
            mockSession.SetupSet(c => c[marketplace] = It.IsAny<string>()).Callback<string,object>((val,obj) => mockSession.SetupGet(c => c[marketplace]).Returns(obj));
            mockHttpContext.SetupGet(c => c.Session).Returns(mockSession.Object);

            return mockHttpContext.Object;
        }


        /// <summary>
        /// Create a HttpContext with session
        /// </summary>
        /// <returns></returns>
        public static HttpSessionState FakeHttpSessionState(Preset preset)
        {
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            var sess = (HttpSessionState)typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            sess[preset.Marketplace] = CurrentMnoSession;

            return sess;
        }
    }

    // Stub class for Saml.Response
    public class SsoResponseStub : Response
    {
        public SsoResponseStub()
        {
            _cachedAttributes = new NameValueCollection();
            _cachedAttributes.Add("mno_session", "7ds8f9789a7fd7x0b898bvb8vc9h0gg");
            _cachedAttributes.Add("mno_session_recheck", DateTime.UtcNow.ToString("s"));

            _cachedAttributes.Add("group_uid", "cld-1");
            _cachedAttributes.Add("group_name", "SomeGroupName");
            _cachedAttributes.Add("group_email", "email@example.com");
            _cachedAttributes.Add("group_role", "Admin");
            _cachedAttributes.Add("group_end_free_trial", DateTime.UtcNow.ToString("s"));
            _cachedAttributes.Add("group_has_credit_card", "true");
            _cachedAttributes.Add("group_currency", "USD");
            _cachedAttributes.Add("group_timezone", "America/Los_Angeles");
            _cachedAttributes.Add("group_country", "US");
            _cachedAttributes.Add("group_city", "Los Angeles");

            _cachedAttributes.Add("uid", "usr-1");
            _cachedAttributes.Add("virtual_uid", "user-1.cld-1");
            _cachedAttributes.Add("email", "j.doe@doecorp.com");
            _cachedAttributes.Add("virtual_email", "user-1.cld-1@mail.maestrano.com");
            _cachedAttributes.Add("name", "John");
            _cachedAttributes.Add("surname", "Doe");
            _cachedAttributes.Add("country", "AU");
            _cachedAttributes.Add("company_name", "DoeCorp");

        }
    }
}
