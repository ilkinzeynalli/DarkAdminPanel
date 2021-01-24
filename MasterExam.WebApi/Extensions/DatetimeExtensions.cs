using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Extensions
{
    public static class DatetimeExtensions
    {
        public static DateTime ConvertUtcToLocalTime(this DateTime datetime)
        {
            DateTime convertedDate = DateTime.SpecifyKind(
                            DateTime.Parse(datetime.ToString()),
                            DateTimeKind.Utc);

            return convertedDate.ToLocalTime();
        }
    }
}
