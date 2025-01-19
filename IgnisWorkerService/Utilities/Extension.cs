using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnisWorkerService.Utilities
{
    public static class Extension
    {
        public static decimal ToDecimalFromString(this string val)
        {
            try
            {
                decimal dec = 0;
                var valid = decimal.TryParse(val, out dec);
                return dec;               
            }catch { return 0; }
        }

        public static DateTime ToDateTimeUtcFromString(this string val)
        {
            if(!string.IsNullOrEmpty(val))
            {
                try
                {
                    // Define Central Time zone
                    TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

                    // Parse the string into a DateTime object
                    DateTime centralTime;
                    bool valid = DateTime.TryParse(val, out centralTime);

                    if (valid)
                    {
                        // Convert Central Time to UTC
                        return TimeZoneInfo.ConvertTimeToUtc(centralTime, centralTimeZone);
                    }
                }
                catch
                {
                    return DateTime.UtcNow;
                }
                
                   

                
            }
            return DateTime.UtcNow;

        }
    }
}
