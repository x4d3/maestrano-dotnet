using System;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Web;
using Maestrano.Sso;
using System.Configuration;
using System.Collections.Specialized;

namespace Maestrano.Tests
{
    [TestClass]
    public class MaestranoMethodsTests
    {
        [TestMethod]
        public void Authenticate_ItReturnsTheRightValues()
        {
            MnoHelper.Environment = "production";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "somekey";

            Assert.AreEqual(true, MnoHelper.Authenticate(MnoHelper.Api.Id, MnoHelper.Api.Key));
            Assert.AreEqual(false, MnoHelper.Authenticate(MnoHelper.Api.Id, "someotherrandomkey"));
        }

        [TestMethod]
        public void UnmaskUser_ItReturnsTheRightValues()
        {
            MnoHelper.Environment = "production";
            Assert.AreEqual("usr-1", MnoHelper.UnmaskUser("usr-1"));
            Assert.AreEqual("usr-1", MnoHelper.UnmaskUser("usr-1.cld-4"));
        }

        [TestMethod]
        public void MaskUser_WhenRealMode_ItReturnsTheRightValues()
        {
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "real";

            Assert.AreEqual("usr-1", MnoHelper.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void MaskUser_WhenVirtualMode_ItReturnsTheRightValues()
        {
            MnoHelper.Environment = "production";
            MnoHelper.Sso.CreationMode = "virtual";

            Assert.AreEqual("usr-1.cld-1", MnoHelper.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void ToMetadata_ItReturnsTheRightObject()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "somekey";

            JObject expected = new JObject(
                new JProperty("environment", MnoHelper.Environment),
                new JProperty("app", new JObject(new JProperty("host",MnoHelper.App.Host))),
                new JProperty("api", new JObject(
                    new JProperty("id",MnoHelper.Api.Id), 
                    new JProperty("lang",MnoHelper.Api.Lang),
                    new JProperty("version",MnoHelper.Api.Version),
                    new JProperty("lang_version",MnoHelper.Api.LangVersion))),
                new JProperty("sso", new JObject(
                    new JProperty("enabled", MnoHelper.Sso.Enabled),
                    new JProperty("creation_mode", MnoHelper.Sso.CreationMode),
                    new JProperty("init_path", MnoHelper.Sso.InitPath),
                    new JProperty("consume_path", MnoHelper.Sso.ConsumePath),
                    new JProperty("idm", MnoHelper.Sso.Idm),
                    new JProperty("idp", MnoHelper.Sso.Idp),
                    new JProperty("name_id_format", MnoHelper.Sso.NameIdFormat),
                    new JProperty("x509_fingerprint", MnoHelper.Sso.X509Fingerprint),
                    new JProperty("x509_certificate", MnoHelper.Sso.X509Certificate))),
                new JProperty("webhook", new JObject(
                    new JProperty("account", new JObject(
                        new JProperty("groups_path", MnoHelper.Webhook.Account.GroupsPath),
                        new JProperty("group_users_path", MnoHelper.Webhook.Account.GroupUsersPath)
                        ))))
            );

            Assert.AreEqual(expected.ToString(), MnoHelper.ToMetadata().ToString());
        }

        [TestMethod]
        public void Sso_SessionCheckUrl_ItsReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";

            Assert.AreEqual(MnoHelper.Sso.Idp + "/api/v1/auth/saml/usr-1?session=somesessiontoken", MnoHelper.Sso.SessionCheckUrl("usr-1", "somesessiontoken"));
        }

        [TestMethod]
        public void Sso_IdpUrl_ItsReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(MnoHelper.Sso.Idp + "/api/v1/auth/saml", MnoHelper.Sso.IdpUrl());
        }

        [TestMethod]
        public void Sso_InitUrl_ItReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(MnoHelper.Sso.Idm + MnoHelper.Sso.InitPath, MnoHelper.Sso.InitUrl());
        }
        

        [TestMethod]
        public void Sso_ConsumeUrl_ItReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(MnoHelper.Sso.Idm + MnoHelper.Sso.ConsumePath, MnoHelper.Sso.ConsumeUrl());
        }

        [TestMethod]
        public void Sso_LogoutUrl_ItReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(MnoHelper.Sso.Idp + "/app_logout", MnoHelper.Sso.LogoutUrl());
        }

        [TestMethod]
        public void Sso_UnauthorizedUrl_ItReturnsTheRightUrl()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(MnoHelper.Sso.Idp + "/app_access_unauthorized", MnoHelper.Sso.UnauthorizedUrl());
        }

        [TestMethod]
        public void Sso_SamlSettings_ItReturnsTheRightSamlSettings()
        {
            MnoHelper.Environment = "production";
            MnoHelper.App.Host = "https://mysuperapp.com";
            MnoHelper.Api.Id = "app-1";

            Settings settings = MnoHelper.Sso.SamlSettings();

            Assert.AreEqual("https://maestrano.com/api/v1/auth/saml", settings.IdpSsoTargetUrl);
            Assert.AreEqual(MnoHelper.Api.Id, settings.Issuer);
            Assert.AreEqual(MnoHelper.Sso.NameIdFormat, settings.NameIdentifierFormat);
            Assert.AreEqual(MnoHelper.Sso.Idm + MnoHelper.Sso.ConsumePath, settings.AssertionConsumerServiceUrl);
            Assert.AreEqual(MnoHelper.Sso.X509Certificate, settings.IdpCertificate);
        }

        [TestMethod]
        public void Sso_BuildRequest_ItShouldReturnSamlRequest()
        {
            MnoHelper.Environment = "production";
            Assert.IsInstanceOfType(MnoHelper.Sso.BuildRequest(), typeof(Request));
        }

        [TestMethod]
        public void Sso_BuildResponse_ItShouldReturnSamlRequest()
        {
            MnoHelper.Environment = "production";
            string samlResponseStr = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Assert.IsInstanceOfType(MnoHelper.Sso.BuildResponse(samlResponseStr), typeof(Response));
        }

        [TestMethod]
        public void Sso_SetSession_ItShouldSetTheUserInSession()
        {
            MnoHelper.Environment = "production";

            // Build context
            var httpContext = Helpers.FakeHttpContext();
            var samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var user = new User(samlResp);
            MnoHelper.Sso.SetSession(httpContext.Session, user);

            // Decrypt session
            var enc = System.Text.Encoding.UTF8;
            var json = enc.GetString(Convert.FromBase64String(httpContext.Session["maestrano"].ToString()));
            var mnoObj = JObject.Parse(json);

            Assert.AreEqual(user.SsoSession, mnoObj.Value<String>("session"));
            Assert.AreEqual(user.Uid, mnoObj.Value<String>("uid"));
            Assert.AreEqual(user.GroupUid, mnoObj.Value<String>("group_uid"));
            Assert.AreEqual(user.SsoSessionRecheck, mnoObj.Value<DateTime>("session_recheck"));
        }

        [TestMethod]
        public void Sso_ClearSession_ItShouldDeleteTheMaestranoSession()
        {
            MnoHelper.Environment = "production";

            // Build context
            var httpContext = Helpers.FakeHttpContext();
            var samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var user = new User(samlResp);
            MnoHelper.Sso.SetSession(httpContext.Session, user);

            // Test
            MnoHelper.Sso.ClearSession(httpContext.Session);
            Assert.IsNull(httpContext.Session["maestrano"]);
            
        }
    }
}
