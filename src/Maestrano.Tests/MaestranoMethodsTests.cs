using System;
using Maestrano.Saml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Maestrano.Tests
{
    [TestClass]
    public class MaestranoMethodsTests
    {
        [TestMethod]
        public void Authenticate_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Api.Id = "app-1";
            Maestrano.Api.Key = "somekey";

            Assert.AreEqual(true, Maestrano.Authenticate(Maestrano.Api.Id, Maestrano.Api.Key));
            Assert.AreEqual(false, Maestrano.Authenticate(Maestrano.Api.Id, "someotherrandomkey"));
        }

        [TestMethod]
        public void UnmaskUser_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Assert.AreEqual("usr-1", Maestrano.UnmaskUser("usr-1"));
            Assert.AreEqual("usr-1", Maestrano.UnmaskUser("usr-1.cld-4"));
        }

        [TestMethod]
        public void MaskUser_WhenRealMode_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "real";

            Assert.AreEqual("usr-1", Maestrano.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void MaskUser_WhenVirtualMode_ItReturnsTheRightValues()
        {
            Maestrano.Environment = "production";
            Maestrano.Sso.CreationMode = "virtual";

            Assert.AreEqual("usr-1.cld-1", Maestrano.MaskUser("usr-1", "cld-1"));
        }

        [TestMethod]
        public void ToMetadata_ItReturnsTheRightObject()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";
            Maestrano.Api.Id = "app-1";
            Maestrano.Api.Key = "somekey";

            JObject expected = new JObject(
                new JProperty("environment", Maestrano.Environment),
                new JProperty("app", new JObject(new JProperty("host",Maestrano.App.Host))),
                new JProperty("api", new JObject(
                    new JProperty("id",Maestrano.Api.Id), 
                    new JProperty("lang",Maestrano.Api.Lang),
                    new JProperty("version",Maestrano.Api.Version),
                    new JProperty("lang_version",Maestrano.Api.LangVersion))),
                new JProperty("sso", new JObject(
                    new JProperty("enabled", Maestrano.Sso.Enabled),
                    new JProperty("creation_mode", Maestrano.Sso.CreationMode),
                    new JProperty("init_path", Maestrano.Sso.InitPath),
                    new JProperty("consume_path", Maestrano.Sso.ConsumePath),
                    new JProperty("idm", Maestrano.Sso.Idm),
                    new JProperty("idp", Maestrano.Sso.Idp),
                    new JProperty("name_id_format", Maestrano.Sso.NameIdFormat),
                    new JProperty("x509_fingerprint", Maestrano.Sso.X509Fingerprint),
                    new JProperty("x509_certificate", Maestrano.Sso.X509Certificate))),
                new JProperty("webhook", new JObject(
                    new JProperty("account", new JObject(
                        new JProperty("groups_path", Maestrano.Webhook.Account.GroupsPath),
                        new JProperty("group_users_path", Maestrano.Webhook.Account.GroupUsersPath)
                        ))))
            );

            Assert.AreEqual(expected.ToString(), Maestrano.ToMetadata().ToString());
        }

        [TestMethod]
        public void Sso_SessionCheckUrl_ItsReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";

            Assert.AreEqual(Maestrano.Sso.Idp + "/api/v1/auth/saml/usr-1?session=somesessiontoken", Maestrano.Sso.SessionCheckUrl("usr-1", "somesessiontoken"));
        }

        [TestMethod]
        public void Sso_IdpUrl_ItsReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Maestrano.Sso.Idp + "/api/v1/auth/saml", Maestrano.Sso.IdpUrl());
        }

        [TestMethod]
        public void Sso_InitUrl_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Maestrano.Sso.Idm + Maestrano.Sso.InitPath, Maestrano.Sso.InitUrl());
        }
        

        [TestMethod]
        public void Sso_ConsumeUrl_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Maestrano.Sso.Idm + Maestrano.Sso.ConsumePath, Maestrano.Sso.ConsumeUrl());
        }

        [TestMethod]
        public void Sso_LogoutUrl_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Maestrano.Sso.Idp + "/app_logout", Maestrano.Sso.LogoutUrl());
        }

        [TestMethod]
        public void Sso_UnauthorizedUrl_ItReturnsTheRightUrl()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";

            Assert.AreEqual(Maestrano.Sso.Idp + "/app_access_unauthorized", Maestrano.Sso.UnauthorizedUrl());
        }

        [TestMethod]
        public void Sso_SamlSettings_ItReturnsTheRightSamlSettings()
        {
            Maestrano.Environment = "production";
            Maestrano.App.Host = "https://mysuperapp.com";
            Maestrano.Api.Id = "app-1";

            Settings settings = Maestrano.Sso.SamlSettings();

            Assert.AreEqual("https://maestrano.com/api/v1/auth/saml", settings.IdpSsoTargetUrl);
            Assert.AreEqual(Maestrano.Api.Id, settings.Issuer);
            Assert.AreEqual(Maestrano.Sso.NameIdFormat, settings.NameIdentifierFormat);
            Assert.AreEqual(Maestrano.Sso.Idm + Maestrano.Sso.ConsumePath, settings.AssertionConsumerServiceUrl);
            Assert.AreEqual(Maestrano.Sso.X509Certificate, settings.IdpCertificate);
        }

        [TestMethod]
        public void Sso_BuildRequest_ItShouldReturnSamlRequest()
        {
            Maestrano.Environment = "production";
            Assert.IsInstanceOfType(Maestrano.Sso.BuildRequest(), typeof(Request));
        }

        [TestMethod]
        public void Sso_BuildResponse_ItShouldReturnSamlRequest()
        {
            Maestrano.Environment = "production";
            string samlResponseStr = Helpers.ReadSamlSupportFiles("Responses/response1.xml.base64");
            Assert.IsInstanceOfType(Maestrano.Sso.BuildResponse(samlResponseStr), typeof(Response));
        }
    }
}
