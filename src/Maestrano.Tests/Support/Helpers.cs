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

namespace Maestrano.Tests
{
    public class Helpers
    {

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
            //httpSession["maestrano"] = Convert.ToBase64String(enc.GetBytes(mnoContent.ToString()));
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

            var enc = System.Text.Encoding.UTF8;
            CurrentMnoSession = Convert.ToBase64String(enc.GetBytes(mnoContent.ToString()));

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

            // Read the file
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(responsePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            string result = sb.ToString();

            // Return as string
            return sb.ToString();
        }

        /// <summary>
        /// Create a HttpContext with session
        /// </summary>
        /// <returns></returns>
        public static HttpContextBase FakeHttpContext()
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockSession.SetupGet(c => c["maestrano"]).Returns(CurrentMnoSession);
            mockSession.SetupSet(c => c["maestrano"] = It.IsAny<string>()).Callback<string,object>((val,obj) => mockSession.SetupGet(c => c["maestrano"]).Returns(obj));
            mockHttpContext.SetupGet(c => c.Session).Returns(mockSession.Object);

            return mockHttpContext.Object;
        }


        /// <summary>
        /// Create a HttpContext with session
        /// </summary>
        /// <returns></returns>
        public static HttpSessionState FakeHttpSessionState()
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

            sess["maestrano"] = CurrentMnoSession;

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
