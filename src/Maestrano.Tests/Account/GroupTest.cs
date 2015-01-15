using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maestrano.Account;

namespace Maestrano.Tests.Account
{
    [TestClass]
    public class GroupTest
    {
        public GroupTest()
        {
            MnoHelper.Environment = "test";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [TestMethod]
        public void All_ItShouldReturnTheListOfGroups()
        {
            var list = Group.All();
            Assert.IsTrue(list[0].Id.Contains("cld-"));
            Assert.IsTrue(list[1].Id.Contains("cld-"));
        }

        [TestMethod]
        public void Retrieve_ItShouldReturnASingleGroup()
        {
            var obj = Group.Retrieve("cld-3");
            Assert.AreEqual("cld-3", obj.Id);
        }

        [TestMethod]
        public void TimeZone_ItRetrievesATimeZoneObject()
        {
            var obj = Group.Retrieve("cld-3");
            Assert.AreEqual(typeof(TimeZoneInfo),obj.TimeZone.GetType());
        }
    }
}
