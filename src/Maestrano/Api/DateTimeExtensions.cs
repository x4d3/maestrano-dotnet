using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Collections;
using System.Reflection;
using System.Web.Script.Serialization;

namespace Maestrano.Api
{
    public static class DateTimeExtensions
    {
        private static DateTimeOffset epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToUnixEpoch(this DateTimeOffset dt)
        {
            dt = dt.ToUniversalTime();
            return Convert.ToInt64((dt - epoch).TotalSeconds);
        }

        public static DateTimeOffset ToDateTime(this long l)
        {
            return epoch.AddSeconds(l);
        }
    }
}
