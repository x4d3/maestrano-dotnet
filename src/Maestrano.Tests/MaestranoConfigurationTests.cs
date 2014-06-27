using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Maestrano.Tests
{
    [TestClass]
    public class MaestranoConfigurationTests
    {
        [TestMethod]
        public void itHasTheRightDefaultTestConfig()
        {
            Maestrano.Environment = "test";

            // App
            Assert.AreEqual("http://localhost", Maestrano.App.Host);

            // API
            Assert.AreEqual("http://api-sandbox.maestrano.io", Maestrano.Api.Host);
            Assert.AreEqual("/api/v1/", Maestrano.Api.Base);
            Assert.AreEqual("C#", Maestrano.Api.Lang);
            Assert.AreEqual(Maestrano.Version, Maestrano.Api.Version);
            Assert.AreEqual(Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(), Maestrano.Api.LangVersion);

            // Webhook
            Assert.AreEqual("/maestrano/account/groups/:id", Maestrano.Webhook.Account.GroupsPath);
            Assert.AreEqual("/maestrano/account/groups/:group_id/users/:id", Maestrano.Webhook.Account.GroupUsersPath);

            // SSO
            Assert.IsTrue(Maestrano.Sso.Enabled);
            Assert.IsTrue(Maestrano.Sso.SloEnabled);
            Assert.AreEqual("http://api-sandbox.maestrano.io", Maestrano.Sso.Idp);
            Assert.AreEqual("http://localhost", Maestrano.Sso.Idm);
            Assert.AreEqual("virtual", Maestrano.Sso.CreationMode);
            Assert.AreEqual("/maestrano/auth/saml/init.aspx", Maestrano.Sso.InitPath);
            Assert.AreEqual("/maestrano/auth/saml/consume.aspx", Maestrano.Sso.ConsumePath);
            Assert.AreEqual("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent", Maestrano.Sso.NameIdFormat);
            Assert.AreEqual("01:06:15:89:25:7d:78:12:28:a6:69:c7:de:63:ed:74:21:f9:f5:36", Maestrano.Sso.X509Fingerprint);
            Assert.AreEqual("-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAOehBr+YIrhjMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyMjM5\nWhcNMzMxMjMwMDUyMjM5WjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDVkIqo5t5Paflu\nP2zbSbzxn29n6HxKnTcsubycLBEs0jkTkdG7seF1LPqnXl8jFM9NGPiBFkiaR15I\n5w482IW6mC7s8T2CbZEL3qqQEAzztEPnxQg0twswyIZWNyuHYzf9fw0AnohBhGu2\n28EZWaezzT2F333FOVGSsTn1+u6tFwIDAQABo4HuMIHrMB0GA1UdDgQWBBSvrNxo\neHDm9nhKnkdpe0lZjYD1GzCBuwYDVR0jBIGzMIGwgBSvrNxoeHDm9nhKnkdpe0lZ\njYD1G6GBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA56EGv5giuGMwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQCc\nMPgV0CpumKRMulOeZwdpnyLQI/NTr3VVHhDDxxCzcB0zlZ2xyDACGnIG2cQJJxfc\n2GcsFnb0BMw48K6TEhAaV92Q7bt1/TYRvprvhxUNMX2N8PHaYELFG2nWfQ4vqxES\nRkjkjqy+H7vir/MOF3rlFjiv5twAbDKYHXDT7v1YCg==\n-----END CERTIFICATE-----", Maestrano.Sso.X509Certificate);
        }

        [TestMethod]
        public void itHasTheRightDefaultProductionConfig()
        {
            Maestrano.Environment = "production";

            // App
            Assert.AreEqual("http://localhost", Maestrano.App.Host);

            // API
            Assert.AreEqual("https://maestrano.com", Maestrano.Api.Host);
            Assert.AreEqual("/api/v1/", Maestrano.Api.Base);
            Assert.AreEqual("C#", Maestrano.Api.Lang);
            Assert.AreEqual(Maestrano.Version, Maestrano.Api.Version);
            Assert.AreEqual(Environment.OSVersion.ToString() + " - " + Environment.Version.ToString(), Maestrano.Api.LangVersion);

            // Webhook
            Assert.AreEqual("/maestrano/account/groups/:id", Maestrano.Webhook.Account.GroupsPath);
            Assert.AreEqual("/maestrano/account/groups/:group_id/users/:id", Maestrano.Webhook.Account.GroupUsersPath);

            // SSO
            Assert.IsTrue(Maestrano.Sso.Enabled);
            Assert.IsTrue(Maestrano.Sso.SloEnabled);
            Assert.AreEqual("https://maestrano.com", Maestrano.Sso.Idp);
            Assert.AreEqual("http://localhost", Maestrano.Sso.Idm);
            Assert.AreEqual("virtual", Maestrano.Sso.CreationMode);
            Assert.AreEqual("/maestrano/auth/saml/init.aspx", Maestrano.Sso.InitPath);
            Assert.AreEqual("/maestrano/auth/saml/consume.aspx", Maestrano.Sso.ConsumePath);
            Assert.AreEqual("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent", Maestrano.Sso.NameIdFormat);
            Assert.AreEqual("2f:57:71:e4:40:19:57:37:a6:2c:f0:c5:82:52:2f:2e:41:b7:9d:7e", Maestrano.Sso.X509Fingerprint);
            Assert.AreEqual("-----BEGIN CERTIFICATE-----\nMIIDezCCAuSgAwIBAgIJAPFpcH2rW0pyMA0GCSqGSIb3DQEBBQUAMIGGMQswCQYD\nVQQGEwJBVTEMMAoGA1UECBMDTlNXMQ8wDQYDVQQHEwZTeWRuZXkxGjAYBgNVBAoT\nEU1hZXN0cmFubyBQdHkgTHRkMRYwFAYDVQQDEw1tYWVzdHJhbm8uY29tMSQwIgYJ\nKoZIhvcNAQkBFhVzdXBwb3J0QG1hZXN0cmFuby5jb20wHhcNMTQwMTA0MDUyNDEw\nWhcNMzMxMjMwMDUyNDEwWjCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEP\nMA0GA1UEBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQG\nA1UEAxMNbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVz\ndHJhbm8uY29tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD3feNNn2xfEz5/\nQvkBIu2keh9NNhobpre8U4r1qC7h7OeInTldmxGL4cLHw4ZAqKbJVrlFWqNevM5V\nZBkDe4mjuVkK6rYK1ZK7eVk59BicRksVKRmdhXbANk/C5sESUsQv1wLZyrF5Iq8m\na9Oy4oYrIsEF2uHzCouTKM5n+O4DkwIDAQABo4HuMIHrMB0GA1UdDgQWBBSd/X0L\n/Pq+ZkHvItMtLnxMCAMdhjCBuwYDVR0jBIGzMIGwgBSd/X0L/Pq+ZkHvItMtLnxM\nCAMdhqGBjKSBiTCBhjELMAkGA1UEBhMCQVUxDDAKBgNVBAgTA05TVzEPMA0GA1UE\nBxMGU3lkbmV5MRowGAYDVQQKExFNYWVzdHJhbm8gUHR5IEx0ZDEWMBQGA1UEAxMN\nbWFlc3RyYW5vLmNvbTEkMCIGCSqGSIb3DQEJARYVc3VwcG9ydEBtYWVzdHJhbm8u\nY29tggkA8WlwfatbSnIwDAYDVR0TBAUwAwEB/zANBgkqhkiG9w0BAQUFAAOBgQDE\nhe/18oRh8EqIhOl0bPk6BG49AkjhZZezrRJkCFp4dZxaBjwZTddwo8O5KHwkFGdy\nyLiPV326dtvXoKa9RFJvoJiSTQLEn5mO1NzWYnBMLtrDWojOe6Ltvn3x0HVo/iHh\nJShjAn6ZYX43Tjl1YXDd1H9O+7/VgEWAQQ32v8p5lA==\n-----END CERTIFICATE-----", Maestrano.Sso.X509Certificate);
        }

        [TestMethod]
        public void itSetsTheHostAndIdmProperlyIfDefined()
        {
            Maestrano.Environment = "production";

            string expected = "https://mysuperapp.com";
            Maestrano.App.Host = expected;

            Assert.AreEqual(expected, Maestrano.App.Host);
            Assert.AreEqual(expected, Maestrano.Sso.Idm);
        }

        [TestMethod]
        public void itSetsTheApiTokenProperly()
        {
            Maestrano.Environment = "production";
            Maestrano.Api.Id = "app-1";
            Maestrano.Api.Key = "bla";

            Assert.AreEqual("app-1:bla", Maestrano.Api.Token);
        }
    }
}
