using System;
using System.Globalization;
using Diabetto.iOS.Constants;
using MvvmCross.Converters;

namespace Diabetto.iOS.Converters
{
    public sealed class BreadUnitsMvxValueConverter : MvxValueConverter<float, string>
    {
        public static readonly BreadUnitsMvxValueConverter Instance = new BreadUnitsMvxValueConverter();

        protected override string Convert(float value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString(NumberFormats.BreadUnits, CultureInfo.InvariantCulture);
        }
    }
}