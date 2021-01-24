using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi
{
    public static class DatetimeExtensions
    {
        public static DateTime GetLocalTime(this DateTime datetime,DateTime willConvertDate)
        {
            DateTime convertedDate = DateTime.SpecifyKind(
                            DateTime.Parse(willConvertDate.ToString()),
                            DateTimeKind.Utc);

            return convertedDate.ToLocalTime();
        } 
    }
}
