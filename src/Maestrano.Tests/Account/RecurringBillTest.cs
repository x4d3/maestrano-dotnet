using System;
using NUnit.Framework;
using Maestrano.Account;
using Maestrano.Api;

namespace Maestrano.Tests.Account
{
    [TestFixture]
    public class RecurringBillTest
    {
        public RecurringBillTest()
        {
            MnoHelper.Environment = "development";
            MnoHelper.Api.Id = "app-1";
            MnoHelper.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [Test]
        public void All_ItShouldReturnTheListOfBills()
        {
            var list = RecurringBill.All();
            Assert.AreEqual("rbill-1", list[0].Id);
            Assert.AreEqual("rbill-2", list[1].Id);
        }

        [Test]
        public void Retrieve_ItShouldReturnASingleBill()
        {
            var obj = RecurringBill.Retrieve("rbill-1");
            Assert.AreEqual("rbill-1", obj.Id);
        }

        [Test]
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

        [Test]
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
