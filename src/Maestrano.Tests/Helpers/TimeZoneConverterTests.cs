using NUnit.Framework;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maestrano.Helpers;

namespace Maestrano.Tests.MnoHelpers
{
    [TestFixture]
    class TimeZoneConverterTests
    {

        [Test]
        public void fromOlsonTz_itReturnsANativeTimeZone()
        {
            var tz = TimeZoneConverter.fromOlsonTz("Europe/Rome");

            Assert.True(tz is TimeZoneInfo);
            Assert.AreEqual("W. Europe Standard Time", tz.Id);
        }

        [Test]
        public void fromOlsonTz_itReturnsPacificTimeWithUnknownTimeZones()
        {
            var tz = TimeZoneConverter.fromOlsonTz("SomeInvalid/TimeZone");

            Assert.True(tz is TimeZoneInfo);
            Assert.AreEqual("Pacific Standard Time", tz.Id);
        }

    }

    
}
