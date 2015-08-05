using System;
using NUnit.Framework;
using Maestrano.Account;
using Newtonsoft.Json;

namespace Maestrano.Tests.Account
{
    [TestFixture]
    public class BillTest
    {
        public BillTest()
        {
            MnoHelper.Environment = "development";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [Test]
        public void All_ItShouldReturnTheListOfBills()
        {
            var list = Bill.All();
            Assert.AreEqual("bill-1", list[0].Id);
            Assert.AreEqual("bill-2", list[1].Id);
        }

        [Test]
        public void Retrieve_ItShouldReturnASingleBill()
        {
            var obj = Bill.Retrieve("bill-1");
            Assert.AreEqual("bill-1", obj.Id);
        }

        [Test]
        public void Create_ItShouldCreateABill()
        {
            var obj = Bill.Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            Assert.IsNotNull(obj.Id);
        }

        [Test]
        public void Cancel_ItShouldCancelABill()
        {
            var obj = Bill.Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            obj.Cancel();
            Assert.AreEqual("cancelled", obj.Status);
        }
    }
}
