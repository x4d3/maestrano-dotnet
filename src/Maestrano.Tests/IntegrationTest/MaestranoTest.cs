using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maestrano.Tests.IntegrationTest
{
    [TestClass]
    public class MaestranoTest
    {
        [TestMethod]
        [ExpectedException(typeof(AutoConfigureException), "Could not authenticate. Check your key and secret.")]
        public void AutoConfigure_invalidCredential_throwsAnAutoConfigureException()
        {
            var host = "https://dev-platform.maestrano.io";
            var path = "/api/config/v1";
            var key = "";
            var secret = "";
            MnoHelper.AutoConfigure(host, path, key, secret);
        }


        [TestMethod]
        [Ignore()]
        public void AutoConfigure_withParameters()
        {
            var host = "https://dev-platform.maestrano.io";
            var path = "/api/config/v1";
            var key = "5c8183d9-f76f-4ada-bb25-9e4643h4a70e";
            var secret = "LS7rHne2pDTZY0YBNq6pxg";
            MnoHelper.AutoConfigure(host, path, key, secret);

        }

        [TestMethod]
        [ExpectedException(typeof(Maestrano.AutoConfigureException), "Is not able to connec to the wrong url.")]
        public void AutoConfigure()
        {
            MnoHelper.AutoConfigure();

        }

    }
}
