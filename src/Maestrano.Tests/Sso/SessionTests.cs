using System;
using System.Web;
using Maestrano;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using Maestrano.Sso;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Maestrano.Tests.Sso
{
    

    [TestClass]
    public class SessionTests
    {

        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        // Used to build HttpSession
        private HttpSessionState buildMnoHttpSession(HttpContext context)
        {
            HttpSessionState httpSession = context.Session;
            JObject mnoContent = new JObject(
                    new JProperty("uid","usr-1"),
                    new JProperty("token","sessiontoken"),
                    new JProperty("group_uid","cld-1"),
                    new JProperty("recheck","2014-06-22T01:00:00Z")
                );

            var enc = System.Text.Encoding.UTF8;
            httpSession["maestrano"] = Convert.ToBase64String(enc.GetBytes(mnoContent.ToString()));

            return httpSession;
        }

        [TestMethod]
        public void ItContructsAnInstanceFromHttpSessionStateObject()
        {

            HttpSessionState httpSession = buildMnoHttpSession(FakeHttpContext());
            Session mnoSession = new Session(httpSession);
        }
    }
}
