﻿using System;

namespace AccountingNotebook.Utils
{
    public static class TimestampExtension
    {
        public static DateTime ConvertFromUnixTimestamp(this long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddMilliseconds(timestamp);
        }

        public static long ConvertToUnixTimestamp(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Convert.ToInt64(diff.TotalMilliseconds);
        }
    }
}
