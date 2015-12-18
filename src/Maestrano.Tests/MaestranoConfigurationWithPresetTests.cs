using NUnit.Framework;
using System;

namespace Maestrano.Tests
{
    [TestFixture]
    public class MaestranoConfigurationWithPresetTests
    {

        [Test]
        public void itHasTheRightDefaultTestConfig()
        {
            MnoHelper.ClearPreset("sometenant");
            MnoHelper.Environment = "development";

            // App
            Assert.AreEqual("http://myapp.com", MnoHelper.With("sometenant").App.Host);

            // API
            Assert.AreEqual("http://api-sandbox.maestrano.io", MnoHelper.With("sometenant").Api.Host);
            Assert.AreEqual("/api/v1/", MnoHelper.With("sometenant").Api.Base);
            Assert.AreEqual("C#", MnoHelper.With("sometenant").Api.Lang);
            Assert.AreEqual(MnoHelper.Version, MnoHelper.With("sometenant").Api.Version);
            Assert.AreEqual(Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(), MnoHelper.With("sometenant").Api.LangVersion);

            // SSO
            Assert.IsTrue(MnoHelper.With("sometenant").Sso.Enabled);
            Assert.IsTrue(MnoHelper.With("sometenant").Sso.SloEnabled);
            Assert.AreEqual("https://idp.sometenant.com", MnoHelper.With("sometenant").Sso.Idp);
            Assert.AreEqual("https://idm.myapp.com", MnoHelper.With("sometenant").Sso.Idm);
            Assert.AreEqual("virtual", MnoHelper.With("sometenant").Sso.CreationMode);
            Assert.AreEqual("/sometenant/auth/saml/init.aspx", MnoHelper.With("sometenant").Sso.InitPath);
            Assert.AreEqual("/sometenant/auth/saml/consume", MnoHelper.With("sometenant").Sso.ConsumePath);
            Assert.AreEqual("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent", MnoHelper.With("sometenant").Sso.NameIdFormat);
            Assert.AreEqual("01:06:15:89:25:7d:78:12:28:a6:69:c7:de:63:ed:74:21:f9:f5:36", MnoHelper.With("sometenant").Sso.X509Fingerprint);
            Assert.AreEqual("-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAOehBr+YIrhjMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyMjM5\nWhcNMzMxMjMwMDUyMjM5WjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDVkIqo5t5Paflu\nP2zbSbzxn29n6HxKnTcsubycLBEs0jkTkdG7seF1LPqnXl8jFM9NGPiBFkiaR15I\n5w482IW6mC7s8T2CbZEL3qqQEAzztEPnxQg0twswyIZWNyuHYzf9fw0AnohBhGu2\n28EZWaezzT2F333FOVGSsTn1+u6tFwIDAQABo4HuMIHrMB0GA1UdDgQWBBSvrNxo\neHDm9nhKnkdpe0lZjYD1GzCBuwYDVR0jBIGzMIGwgBSvrNxoeHDm9nhKnkdpe0lZ\njYD1G6GBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA56EGv5giuGMwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQCc\nMPgV0CpumKRMulOeZwdpnyLQI/NTr3VVHhDDxxCzcB0zlZ2xyDACGnIG2cQJJxfc\n2GcsFnb0BMw48K6TEhAaV92Q7bt1/TYRvprvhxUNMX2N8PHaYELFG2nWfQ4vqxES\nRkjkjqy+H7vir/MOF3rlFjiv5twAbDKYHXDT7v1YCg==\n-----END CERTIFICATE-----", MnoHelper.With("sometenant").Sso.X509Certificate);

            // Connec
            Assert.AreEqual("http://api-sandbox.maestrano.io", MnoHelper.With("sometenant").Connec.Host);
            Assert.AreEqual("/connec/api/v2", MnoHelper.With("sometenant").Connec.BasePath);

            // Webhook
            Assert.AreEqual("/maestrano/account/groups/:id", MnoHelper.With("sometenant").Webhook.Account.GroupsPath);
            Assert.AreEqual("/maestrano/account/groups/:group_id/users/:id", MnoHelper.With("sometenant").Webhook.Account.GroupUsersPath);
            Assert.AreEqual("/maestrano/connec/notifications", MnoHelper.With("sometenant").Webhook.Connec.NotificationsPath);

            // Webhook - Connec! Subscriptions
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Accounts);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Company);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Invoices);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Items);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Organizations);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.Payments);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.People);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.TaxCodes);
            Assert.IsFalse(MnoHelper.With("sometenant").Webhook.Connec.Subscriptions.TaxRates);
        }

        [Test]
        public void itSetsTheHostAndIdmProperlyIfDefined()
        {
            MnoHelper.With("sometenant").Environment = "production";

            string expected = "https://somerandomhost.com";
            MnoHelper.With("sometenant").App.Host = expected;
            MnoHelper.With("sometenant").Sso.Idm = expected;

            Assert.AreEqual(expected, MnoHelper.With("sometenant").App.Host);
            Assert.AreEqual(expected, MnoHelper.With("sometenant").Sso.Idm);
        }

        [Test]
        public void itSetsTheApiTokenProperly()
        {
            MnoHelper.With("sometenant").Environment = "production";
            MnoHelper.With("sometenant").Api.Id = "app-1";
            MnoHelper.With("sometenant").Api.Key = "bla";

            Assert.AreEqual("app-1:bla", MnoHelper.With("sometenant").Api.Token);
        }

        [Test]
        public void itGeneratesTheMetadataWithoutError()
        {
            MnoHelper.With("sometenant").Environment = "production";
            MnoHelper.With("sometenant").Api.Id = "app-1";
            MnoHelper.With("sometenant").Api.Key = "bla";

            Assert.IsNotNull(MnoHelper.With("sometenant").ToMetadata());
        }

        [Test]
        public void Sso_ItBuildsTheRightSamlRequest()
        {
            MnoHelper.With("sometenant").Environment = "production";

            var ssoIdpUrl = MnoHelper.With("sometenant").Sso.BuildRequest(null).RedirectUrl();
            Assert.IsTrue(ssoIdpUrl.StartsWith("https://idp.sometenant.com"));
        }

        [Test]
        public void Sso_ItBuildsTheRightSamlSettings()
        {
            MnoHelper.With("sometenant").Environment = "production";
            MnoHelper.With("sometenant").Api.Id = "app-tenant1";
            MnoHelper.With("sometenant").Sso.Idp = "https://idp.sometenantspecificendpoint.com";
            MnoHelper.With("sometenant").Sso.Idm = "https://somespecificapphost.com";
            MnoHelper.With("sometenant").Sso.ConsumePath = "/somespecifictenant/auth/saml/consume";

            var samlSettings = MnoHelper.With("sometenant").Sso.SamlSettings();
            Assert.AreEqual("app-tenant1", samlSettings.Issuer);
            Assert.AreEqual("https://idp.sometenantspecificendpoint.com/api/v1/auth/saml", samlSettings.IdpSsoTargetUrl);
            Assert.AreEqual("https://somespecificapphost.com/somespecifictenant/auth/saml/consume", samlSettings.AssertionConsumerServiceUrl);
        }

    }
}
