using System;
using System.Globalization;
using Foundation;
using MvvmCross.Converters;

namespace Diabetto.iOS.Converters
{
    public sealed class DateFormatterMvxValueConverter : MvxValueConverter<DateTime, string>
    {
        private readonly NSDateFormatter _dateFormatter;

        public DateFormatterMvxValueConverter()
        {
        }

        public DateFormatterMvxValueConverter(NSDateFormatter dateFormatter)
        {
            _dateFormatter = dateFormatter ?? throw new ArgumentNullException(nameof(dateFormatter));
        }

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return _dateFormatter?.ToString((NSDate) value) ?? value.ToString();
        }
    }
}