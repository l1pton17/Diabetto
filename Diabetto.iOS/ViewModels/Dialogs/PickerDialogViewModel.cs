using System;
using Diabetto.Core.Dialogs;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Diabetto.iOS.ViewModels.Dialogs
{
    public sealed class PickerDialogViewModel<T> : MvxPickerViewModel
    {
        private readonly PickerDialogSource<T> _source;

        /// <inheritdoc />
        public PickerDialogViewModel(
            UIPickerView pickerView,
            PickerDialogSource<T> source)
            : base(pickerView)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }
    }
}