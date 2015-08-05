using System;
using NUnit.Framework;
using Maestrano.Account;
using Newtonsoft.Json;

namespace Maestrano.Tests.Account
{
    [TestFixture]
    public class BillWithPresetTest
    {
        private string presetName = "maestrano";

        public BillWithPresetTest()
        {
            MnoHelper.Environment = "development";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [Test]
        public void All_ItShouldReturnTheListOfBills()
        {
            var list = Bill.With(presetName).All();
            Assert.AreEqual(presetName, list[0].PresetName);
            Assert.AreEqual("bill-1", list[0].Id);
            Assert.AreEqual("bill-2", list[1].Id);
        }

        [Test]
        public void Retrieve_ItShouldReturnASingleBill()
        {
            var obj = Bill.With(presetName).Retrieve("bill-1");
            Assert.AreEqual("bill-1", obj.Id);
            Assert.AreEqual(presetName, obj.PresetName);
        }

        [Test]
        public void Create_ItShouldCreateABill()
        {
            var obj = Bill.With(presetName).Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            Assert.IsNotNull(obj.Id);
            Assert.AreEqual(presetName, obj.PresetName);
        }

        [Test]
        public void Cancel_ItShouldCancelABill()
        {
            var obj = Bill.With(presetName).Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            obj.Cancel();
            Assert.AreEqual("cancelled", obj.Status);
            Assert.AreEqual(presetName, obj.PresetName);
        }
    }
}
