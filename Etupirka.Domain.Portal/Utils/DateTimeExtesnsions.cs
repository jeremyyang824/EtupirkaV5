using System;

namespace Etupirka.Domain.Portal.Utils
{
    public static class DateTimeExtesnsions
    {
        /// <summary>
        /// 当天开始时间
        /// </summary>
        public static DateTime DateBegin(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// 当天结束时间
        /// </summary>
        public static DateTime DateEnd(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                23, 59, 59, dateTime.Kind);
        }
    }
}
