using System;
using System.Globalization;
using Foundation;
using MvvmCross.Converters;

namespace Diabetto.iOS.Converters
{
    public sealed class TimeMvxValueConverter : MvxValueConverter<DateTime, string>
    {
        private static readonly NSDateFormatter _dateFormatter = new NSDateFormatter
        {
            DateStyle = NSDateFormatterStyle.None,
            TimeStyle = NSDateFormatterStyle.Short
        };

        public static readonly TimeMvxValueConverter Instance = new TimeMvxValueConverter();

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return _dateFormatter.StringFor((NSDate) value);
        }
    }
}