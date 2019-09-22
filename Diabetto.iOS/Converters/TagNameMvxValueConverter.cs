using System;
using System.Globalization;
using Diabetto.Core.Models;
using MvvmCross.Converters;

namespace Diabetto.iOS.Converters
{
    public sealed class TagNameValueConverter : MvxValueConverter<Tag, string>
    {
        protected override string Convert(Tag value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? "N/A"
                : value.Name;
        }
    }
}