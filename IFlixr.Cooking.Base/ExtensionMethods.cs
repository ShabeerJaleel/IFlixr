using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.Base
{
    public static class ExtensionMethods
    {
      

        public static DateTime Start(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime End(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59,59);
        }
    }
}
