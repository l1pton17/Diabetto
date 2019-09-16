using System;
using System.Globalization;
using System.Linq;
using UIKit;

namespace Diabetto.iOS.ViewModels.Dialogs
{
    public sealed class AddProductMeasureUIPickerViewModel : UIPickerViewModel
    {
        public static readonly int[] AmountScale = Enumerable
            .Range(1, 1000)
            .Select(v => v * 10)
            .ToArray();

        public static readonly float[] CarbohydratesScale = Enumerable
            .Range(1, 1000)
            .Select(v => v / 10.0f)
            .ToArray();

        public int Amount { get; private set; }
        public float Carbohydrates { get; private set; }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 3;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                    Amount = AmountScale[row];
                    break;

                case 2:
                    Carbohydrates = CarbohydratesScale[row];
                    break;
            }
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            switch (component)
            {
                case 0:
                    return AmountScale.Length;

                case 1:
                    return 1;

                case 2:
                    return CarbohydratesScale.Length;
            }

            return 0;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            switch (component)
            {
                case 0:
                    return AmountScale[row].ToString();

                case 1:
                    return "grams";

                case 2:
                    return CarbohydratesScale[row].ToString("0.0", CultureInfo.InvariantCulture);
            }

            return String.Empty;
        }

        public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
        {
            return pickerView.Frame.Width / 3.0f;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return 40f;
        }
    }
}