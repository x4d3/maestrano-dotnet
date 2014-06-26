using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maestrano.Account;
using Newtonsoft.Json;

namespace Maestrano.Tests.Account
{
    [TestClass]
    public class BillTest
    {
        public BillTest()
        {
            Maestrano.Environment = "test";
            Maestrano.Api.Id = "app-1";
            Maestrano.Api.Key = "gfcmbu8269wyi0hjazk4t7o1sndpvrqxl53e1";
        }

        [TestMethod]
        public void All_ItShouldReturnTheListOfBills()
        {
            Console.WriteLine(Maestrano.Api.Id);
            var list = Bill.All();
            Assert.AreEqual("bill-1", list[0].Id);
            Assert.AreEqual("bill-2", list[1].Id);
        }

        [TestMethod]
        public void Retrieve_ItShouldReturnASingleBill()
        {
            Console.WriteLine(Maestrano.Api.Id);
            var obj = Bill.Retrieve("bill-1");
            Assert.AreEqual("bill-1", obj.Id);
        }

        [TestMethod]
        public void Create_ItShouldCreateABill()
        {
            var obj = Bill.Create(
                groupUid: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            Assert.IsNotNull(obj.Id);
        }

        [TestMethod]
        public void Cancel_ItShouldCancelABill()
        {
            var obj = Bill.Create(
                groupUid: "cld-3",
                priceCents: 1500,
                description: "Some Bill"
                );
            obj.Cancel();
            Assert.AreEqual("cancelled", obj.Status);
        }
    }
}
