using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maestrano.Account;
using Maestrano.Helpers;

namespace Maestrano.Tests.Account
{
    [TestClass]
    public class UserWithPresetTest
    {
        private string presetName = "maestrano";

        public UserWithPresetTest()
        {
            MnoHelper.Environment = "development";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [TestMethod]
        public void All_ItShouldReturnTheListOfUsers()
        {
            var list = User.With(presetName).All();
            Assert.IsTrue(list[0].Id.Contains("usr-"));
            Assert.IsTrue(list[1].Id.Contains("usr-"));
            Assert.AreEqual(presetName, list[0].PresetName);
        }

        [TestMethod]
        public void Retrieve_ItShouldReturnASingleUser()
        {
            var obj = User.With(presetName).Retrieve("usr-1");
            Assert.AreEqual("usr-1", obj.Id);
            Assert.AreEqual(presetName, obj.PresetName);
        }

        [TestMethod]
        public void CheckPassword_ItShouldReturnTheRightValues()
        {
            Assert.IsTrue(User.With(presetName).CheckPassword("usr-1", "password"));
            Assert.IsFalse(User.With(presetName).CheckPassword("usr-1", "invalid_password"));
            Assert.IsTrue(User.With(presetName).CheckPassword("j.doe@company.com", "password"));
            Assert.IsFalse(User.With(presetName).CheckPassword("j.doe@company.com", "invalid_password"));
        }
    }
}
