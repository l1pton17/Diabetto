using System;
using System.Reflection;
using Foundation;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Binding.Target;
using UIKit;

namespace Diabetto.iOS.MvxBindings
{
    public class UtcUIDatePickerDateTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public UtcUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return view.Date.ToDateTimeUtc();
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
            {
                value = DateTime.Now;
            }

            var dateValue = (DateTime) value;
            var nsDate = (NSDate) dateValue;

            return nsDate;
        }
    }
}