using System;
using System.Globalization;

namespace Diabetto.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static string GetMonthName(int month)
        {
            return new DateTime(2010, month, 1)
                .ToString("MMMM", CultureInfo.CurrentUICulture);
        }
    }
}