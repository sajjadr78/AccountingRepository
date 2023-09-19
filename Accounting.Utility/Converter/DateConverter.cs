using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Accounting.Utility.Converter
{
    public static class DateConverter
    {
        public static PersianDateTime ToShamsi(this DateTime value)
        {
            PersianDateTime persianDateTime = new PersianDateTime(value);
            return persianDateTime;
        }
        
        public static DateTime ToMiladi(DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, new PersianCalendar());
        }
    }
}
