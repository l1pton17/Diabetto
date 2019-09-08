using System;
using System.Globalization;
using Diabetto.Core.ViewModels.Calendars;
using MvvmCross.Converters;
using UIKit;

namespace Diabetto.iOS.Converters
{
    public sealed class CalendarCellMvxValueConverter : MvxValueConverter<CalendarCellType, UIColor>
    {
        public static readonly CalendarCellMvxValueConverter Instance = new CalendarCellMvxValueConverter();

        protected override UIColor Convert(CalendarCellType value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CalendarCellType.Normal:
                    return UIColor.FromRGB(245, 241, 230);

                case CalendarCellType.PreviousMonth:
                    return UIColor.FromRGB(150, 150, 150);
            }

            return base.Convert(value, targetType, parameter, culture);
        }
    }
}