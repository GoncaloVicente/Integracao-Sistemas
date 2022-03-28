using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlertsApp
{
    public class Date
    {
        public static DateTime getDate(long unixTime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();

            return dtDateTime;
        }

        public static long getUnixTime(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long unixDateTime = (long)(date.ToUniversalTime() - epoch).TotalSeconds;

            return unixDateTime;
        }
    }
}
