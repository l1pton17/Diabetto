using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Diabetto.iOS.Converters
{
    public sealed class LevelMvxValueConverter : MvxValueConverter<int?, string>
    {
        public string EmptyValue { get; set; }

        public string Prefix { get; set; }

        protected override string Convert(
            int? value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            if (value.HasValue)
            {
                var formatted = (value.Value / 10.0).ToString("0.0", CultureInfo.InvariantCulture);

                return String.IsNullOrEmpty(Prefix)
                    ? formatted
                    : String.Concat(formatted, " ", Prefix);
            }

            return EmptyValue;
        }
    }
}