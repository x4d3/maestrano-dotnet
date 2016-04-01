using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maestrano.Api;

namespace Maestrano.Tests.Api
{
    [TestClass]
    public class MnoClientTest
    {
        public class DateContainer
        {
            public DateTime Date { get; set; }
            public DateTime IncorrectDate { get; set; }
            public DateTime? IncorrectNullableDate { get; set; }
        }

        [TestMethod]
        public void DeserializeObject_DeserializeDate()
        {
            string json = @"{'Date': '0001-01-01T00:00:00Z','IncorrectDate': '0000-01-01T00:00:00Z','IncorrectNullableDate': '-000-01-01T00:00:00Z'}";
            var result = MnoClient.DeserializeObject<DateContainer>(json);
            Assert.AreEqual(new DateTime(1,1,1), result.IncorrectDate);
            Assert.AreEqual(DateTime.MinValue, result.IncorrectDate);
            Assert.IsNull(result.IncorrectNullableDate);
        }
    }
}
