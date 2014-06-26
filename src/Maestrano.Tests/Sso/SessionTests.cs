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
using Moq;
using RestSharp;

namespace Maestrano.Tests.Sso
{
    

    [TestClass]
    public class SessionTests
    {

        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new System.Web.HttpResponse(stringWriter);
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
            return injectMnoSession(context, DateTime.Parse("2014-06-22T01:00:00Z").ToUniversalTime());
        }

        private HttpContext injectMnoSession(HttpContext context, DateTime datetime)
        {
            HttpSessionState httpSession = context.Session;
            JObject mnoContent = new JObject(
                    new JProperty("uid", "usr-1"),
                    new JProperty("session", "sessiontoken"),
                    new JProperty("group_uid", "cld-1"),
                    new JProperty("session_recheck", datetime.ToString("s"))
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

        [TestMethod]
        public void ItContructsAnInstanceFromHttpSessionStateObjectAndSsoUser()
        {
            // Http context
            HttpContext httpContext = FakeHttpContext();
            injectMnoSession(httpContext);

            // User
            SsoUserResponseStub samlResp = new SsoUserResponseStub();
            var user = new User(samlResp);


            Session mnoSession = new Session(httpContext.Session, user);

            Assert.AreEqual(httpContext.Session, mnoSession.HttpSession);
            Assert.AreEqual(user.Uid, mnoSession.Uid);
            Assert.AreEqual(user.GroupUid, mnoSession.GroupUid);
            Assert.AreEqual(user.SsoSession, mnoSession.SessionToken);
            Assert.AreEqual(user.SsoSessionRecheck, mnoSession.Recheck);
        }

        [TestMethod]
        public void IsRemoteCheckRequired_ItReturnsTrueIfRecheckIsBeforeNow()
        {
            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(-1);
            injectMnoSession(httpContext, recheck);

            // test
            Session mnoSession = new Session(httpContext.Session);
            Assert.IsTrue(mnoSession.isRemoteCheckRequired());
        }

        [TestMethod]
        public void IsRemoteCheckRequired_ItReturnsFalseIfRecheckIsAfterNow()
        {
            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(1);
            injectMnoSession(httpContext, recheck);

            // test
            Session mnoSession = new Session(httpContext.Session);
            Assert.IsFalse(mnoSession.isRemoteCheckRequired());
        }

        [TestMethod]
        public void PerformRemoteCheck_WhenValid_ItShouldReturnTrueAndAssignRecheckIfValid()
        {   
            // Response preparation
            RestResponse response = new RestResponse();
            var datetime = DateTime.UtcNow;
            JObject respObj = new JObject(new JProperty("valid", "true"), new JProperty("recheck", datetime.ToString("s")));
            response.Content = respObj.ToString();
            response.ResponseStatus = ResponseStatus.Completed;

            // Client mock
            var mockRestClient = new Mock<RestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>())).Returns(response);

            // Http context
            HttpContext httpContext = FakeHttpContext();
            injectMnoSession(httpContext);
            Session mnoSession = new Session(httpContext.Session);

            // Tests
            Assert.IsTrue(mnoSession.PerformRemoteCheck(mockRestClient.Object));
            Assert.AreEqual(DateTime.Parse(datetime.ToString("s")), mnoSession.Recheck);
        }

        [TestMethod]
        public void PerformRemoteCheck_WhenInvalid_ItShouldReturnFalseAndLeaveRecheckUnchanged()
        {
            // Response preparation
            RestResponse response = new RestResponse();
            var datetime = DateTime.UtcNow;
            JObject respObj = new JObject(new JProperty("valid", "false"), new JProperty("recheck", datetime.ToString("s")));
            response.Content = respObj.ToString();
            response.ResponseStatus = ResponseStatus.Completed;

            // Client mock
            var mockRestClient = new Mock<RestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>())).Returns(response);

            // Http context
            HttpContext httpContext = FakeHttpContext();
            injectMnoSession(httpContext);
            Session mnoSession = new Session(httpContext.Session);

            // Tests
            var recheck = mnoSession.Recheck;
            Assert.IsFalse(mnoSession.PerformRemoteCheck(mockRestClient.Object));
            Assert.AreEqual(DateTime.Parse(recheck.ToString("s")), mnoSession.Recheck);
        }


        [TestMethod]
        public void Save_ItShouldSaveTheMaestranoSessionInHttpSession()
        {
            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(1);
            injectMnoSession(httpContext, recheck);

            // Create Mno session and save it
            Session mnoSession = new Session(httpContext.Session);
            mnoSession.SessionToken = "anothertoken";
            mnoSession.Save();

            // Decrypt session and test
            var enc = System.Text.Encoding.UTF8;
            var json = enc.GetString(Convert.FromBase64String(httpContext.Session["maestrano"].ToString()));
            var mnoObj = JObject.Parse(json);

            Assert.AreEqual(mnoSession.SessionToken, mnoObj.Value<String>("session"));
            Assert.AreEqual(mnoSession.Uid, mnoObj.Value<String>("uid"));
            Assert.AreEqual(mnoSession.GroupUid, mnoObj.Value<String>("group_uid"));
            Assert.AreEqual(mnoSession.Recheck, mnoObj.Value<DateTime>("session_recheck"));
        }

        [TestMethod]
        public void IsValid_WhenRecheckRequired_ItShouldReturnTrue()
        {
            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(1);
            injectMnoSession(httpContext, recheck);

            // test
            Session mnoSession = new Session(httpContext.Session);
            Assert.IsTrue(mnoSession.IsValid());
        }

        [TestMethod]
        public void IsValid_WhenRecheckRequiredAndValid_ItShouldReturnTrueAndSaveTheSession()
        {
            // Response preparation
            RestResponse response = new RestResponse();
            var datetime = DateTime.UtcNow;
            JObject respObj = new JObject(new JProperty("valid", "true"), new JProperty("recheck", datetime.ToString("s")));
            response.Content = respObj.ToString();
            response.ResponseStatus = ResponseStatus.Completed;

            // Client mock
            var mockRestClient = new Mock<RestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>())).Returns(response);

            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(-1);
            injectMnoSession(httpContext, recheck);

            // Test mno session
            Session mnoSession = new Session(httpContext.Session);
            Assert.IsTrue(mnoSession.IsValid(mockRestClient.Object));

            // Decrypt session and test recheck
            var enc = System.Text.Encoding.UTF8;
            var json = enc.GetString(Convert.FromBase64String(httpContext.Session["maestrano"].ToString()));
            var mnoObj = JObject.Parse(json);

            Assert.AreEqual(datetime.ToString("s"), mnoObj.Value<DateTime>("session_recheck").ToString("s"));
        }

        [TestMethod]
        public void IsValid_ItShouldReturnFalseIfRecheckRequiredAndInvalid()
        {
            // Response preparation
            RestResponse response = new RestResponse();
            var datetime = DateTime.UtcNow;
            JObject respObj = new JObject(new JProperty("valid", "false"), new JProperty("recheck", datetime.ToString("s")));
            response.Content = respObj.ToString();
            response.ResponseStatus = ResponseStatus.Completed;

            // Client mock
            var mockRestClient = new Mock<RestClient>();
            mockRestClient.Setup(c => c.Execute(It.IsAny<RestRequest>())).Returns(response);

            // Http context
            HttpContext httpContext = FakeHttpContext();
            var recheck = DateTime.UtcNow.AddMinutes(-1);
            injectMnoSession(httpContext, recheck);

            // test
            Session mnoSession = new Session(httpContext.Session);
            Assert.IsFalse(mnoSession.IsValid(mockRestClient.Object));
        }
    }
}
