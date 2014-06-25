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
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

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
        private HttpContext injectMnoSession(HttpContext context)
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

            return context;
        }

        [TestMethod]
        public void ItContructsAnInstanceFromHttpSessionStateObject()
        {
            HttpContext httpContext = FakeHttpContext();
            injectMnoSession(httpContext);
            Session mnoSession = new Session(httpContext.Session);

            Assert.AreEqual(httpContext.Session, mnoSession.HttpSession);
            Assert.AreEqual("usr-1", mnoSession.Uid);
            Assert.AreEqual("cld-1", mnoSession.GroupUid);
            Assert.AreEqual("sessiontoken", mnoSession.SessionToken);
            Assert.AreEqual(DateTime.Parse("2014-06-22T01:00:00Z").ToUniversalTime(), mnoSession.Recheck);
        }
    }
}
