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
            Mno.Environment = "production";
            Mno.Api.Id = "app-1";
            Mno.Api.Key = "somekey";

            Assert.AreEqual(true, Mno.Authenticate(Mno.Api.Id, Mno.Api.Key));
            Assert.AreEqual(false, Mno.Authenticate(Mno.Api.Id, "someotherrandomkey"));
        }

        [TestMethod]
        public void UnmaskUser_ItReturnsTheRightValues()
        {
            Mno.Environment = "production";
            Assert.AreEqual("usr-1", Mno.UnmaskUser("usr-1"));
            Assert.AreEqual("usr-1", Mno.UnmaskUser("usr-1.cld-4"));
        }

        [TestMethod]
        public void MaskUser_WhenRealMode_ItReturnsTheRightValues()
        {
            Mno.Environment = "production";
            Mno.Sso.CreationMode = "real";

            Assert.AreEqual("usr-1", Mno.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void MaskUser_WhenVirtualMode_ItReturnsTheRightValues()
        {
            Mno.Environment = "production";
            Mno.Sso.CreationMode = "virtual";

            Assert.AreEqual("usr-1.cld-1", Mno.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void ToMetadata_ItReturnsTheRightObject()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";
            Mno.Api.Id = "app-1";
            Mno.Api.Key = "somekey";

            JObject expected = new JObject(
                new JProperty("environment", Mno.Environment),
                new JProperty("app", new JObject(new JProperty("host",Mno.App.Host))),
                new JProperty("api", new JObject(
                    new JProperty("id",Mno.Api.Id), 
                    new JProperty("lang",Mno.Api.Lang),
                    new JProperty("version",Mno.Api.Version),
                    new JProperty("lang_version",Mno.Api.LangVersion))),
                new JProperty("sso", new JObject(
                    new JProperty("enabled", Mno.Sso.Enabled),
                    new JProperty("creation_mode", Mno.Sso.CreationMode),
                    new JProperty("init_path", Mno.Sso.InitPath),
                    new JProperty("consume_path", Mno.Sso.ConsumePath),
                    new JProperty("idm", Mno.Sso.Idm),
                    new JProperty("idp", Mno.Sso.Idp),
                    new JProperty("name_id_format", Mno.Sso.NameIdFormat),
                    new JProperty("x509_fingerprint", Mno.Sso.X509Fingerprint),
                    new JProperty("x509_certificate", Mno.Sso.X509Certificate))),
                new JProperty("webhook", new JObject(
                    new JProperty("account", new JObject(
                        new JProperty("groups_path", Mno.Webhook.Account.GroupsPath),
                        new JProperty("group_users_path", Mno.Webhook.Account.GroupUsersPath)
                        ))))
            );

            Assert.AreEqual(expected.ToString(), Mno.ToMetadata().ToString());
        }

        [TestMethod]
        public void Sso_SessionCheckUrl_ItsReturnsTheRightUrl()
        {
            Mno.Environment = "production";

            Assert.AreEqual(Mno.Sso.Idp + "/api/v1/auth/saml/usr-1?session=somesessiontoken", Mno.Sso.SessionCheckUrl("usr-1", "somesessiontoken"));
        }

        [TestMethod]
        public void Sso_IdpUrl_ItsReturnsTheRightUrl()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Mno.Sso.Idp + "/api/v1/auth/saml", Mno.Sso.IdpUrl());
        }

        [TestMethod]
        public void Sso_InitUrl_ItReturnsTheRightUrl()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Mno.Sso.Idm + Mno.Sso.InitPath, Mno.Sso.InitUrl());
        }
        

        [TestMethod]
        public void Sso_ConsumeUrl_ItReturnsTheRightUrl()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Mno.Sso.Idm + Mno.Sso.ConsumePath, Mno.Sso.ConsumeUrl());
        }

        [TestMethod]
        public void Sso_LogoutUrl_ItReturnsTheRightUrl()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Mno.Sso.Idp + "/app_logout", Mno.Sso.LogoutUrl());
        }

        [TestMethod]
        public void Sso_UnauthorizedUrl_ItReturnsTheRightUrl()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Mno.Sso.Idp + "/app_access_unauthorized", Mno.Sso.UnauthorizedUrl());
        }

        [TestMethod]
        public void Sso_SamlSettings_ItReturnsTheRightSamlSettings()
        {
            Mno.Environment = "production";
            Mno.App.Host = "https://mysuperapp.com";
            Mno.Api.Id = "app-1";

            Settings settings = Mno.Sso.SamlSettings();

            Assert.AreEqual("https://maestrano.com/api/v1/auth/saml", settings.IdpSsoTargetUrl);
            Assert.AreEqual(Mno.Api.Id, settings.Issuer);
            Assert.AreEqual(Mno.Sso.NameIdFormat, settings.NameIdentifierFormat);
            Assert.AreEqual(Mno.Sso.Idm + Mno.Sso.ConsumePath, settings.AssertionConsumerServiceUrl);
            Assert.AreEqual(Mno.Sso.X509Certificate, settings.IdpCertificate);
        }

        [TestMethod]
        public void Sso_BuildRequest_ItShouldReturnSamlRequest()
        {
            Mno.Environment = "production";
            Assert.IsInstanceOfType(Mno.Sso.BuildRequest(), typeof(Request));
        }

        [TestMethod]
        public void Sso_BuildResponse_ItShouldReturnSamlRequest()
        {
            Mno.Environment = "production";
            string samlResponseStr = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Assert.IsInstanceOfType(Mno.Sso.BuildResponse(samlResponseStr), typeof(Response));
        }

        [TestMethod]
        public void Sso_SetSession_ItShouldSetTheUserInSession()
        {
            Mno.Environment = "production";

            // Build context
            var httpContext = Helpers.FakeHttpContext();
            var samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var user = new User(samlResp);
            Mno.Sso.SetSession(httpContext.Session, user);

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
            Mno.Environment = "production";

            // Build context
            var httpContext = Helpers.FakeHttpContext();
            var samlResp = new SsoResponseStub();
            var att = samlResp.GetAttributes();
            var user = new User(samlResp);
            Mno.Sso.SetSession(httpContext.Session, user);

            // Test
            Mno.Sso.ClearSession(httpContext.Session);
            Assert.IsNull(httpContext.Session["maestrano"]);
            
        }
    }
}
