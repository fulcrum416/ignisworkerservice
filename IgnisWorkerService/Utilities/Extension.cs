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
                    //string customFormat = "MM/dd/yyyy hh:mm:ss"; // Custom format
                    DateTime localDt;
                    var valid = DateTime.TryParse(val, out localDt);
                    return localDt.ToUniversalTime();
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
