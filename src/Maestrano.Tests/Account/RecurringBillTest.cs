using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maestrano.Account;
using Maestrano.Api;

namespace Maestrano.Tests.Account
{
    [TestClass]
    public class RecurringBillTest
    {
        public RecurringBillTest()
        {
            MnoHelper.Environment = "test";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [TestMethod]
        public void All_ItShouldReturnTheListOfBills()
        {
            var list = RecurringBill.All();
            Assert.AreEqual("rbill-1", list[0].Id);
            Assert.AreEqual("rbill-2", list[1].Id);
        }

        [TestMethod]
        public void Retrieve_ItShouldReturnASingleBill()
        {
            var obj = RecurringBill.Retrieve("rbill-1");
            Assert.AreEqual("rbill-1", obj.Id);
        }

        [TestMethod]
        public void Create_ItShouldCreateABill()
        {
            var obj = RecurringBill.Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            Assert.IsNotNull(obj.Id);
            Assert.IsNotNull(obj.CreatedAt);
        }

        [TestMethod]
        public void Cancel_ItShouldCancelABill()
        {
            var obj = RecurringBill.Create(
                groupId: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            var updatedAt = obj.UpdatedAt;
            obj.Cancel();
            Assert.AreEqual("cancelled", obj.Status);
            Assert.IsTrue(obj.UpdatedAt.HasValue);
        }
    }
}
