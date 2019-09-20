using System;
using Diabetto.Core.ViewModels.Dialogs;
using Diabetto.iOS.Dialogs;
using MvvmCross.WeakSubscription;
using UIKit;

namespace Diabetto.iOS.ViewModels.Dialogs
{
    public sealed class PickerDialogViewModel : UIPickerViewModel
    {
        private readonly IDialogPickerViewModel _source;
        private readonly PickerDialog<PickerDialogViewModel> _dialog;

        /// <inheritdoc />
        public PickerDialogViewModel(
            PickerDialog<PickerDialogViewModel> dialog,
            IDialogPickerViewModel source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));

            _source.WeakSubscribe(
                nameof(IDialogPickerViewModel.ItemsChanged),
                OnItemsChanged);
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return _source.ComponentCount;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            _source.SelectItem((int) component, (int) row);
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _source.GetRowsCount((int) component);
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return _source.GetRowTitle((int) component, (int) row);
        }

        public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
        {
            return pickerView.Frame.Width / _source.ComponentCount;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return 40f;
        }

        private void OnItemsChanged(object sender, EventArgs eventArgs)
        {
            _dialog.ReloadAllComponents();
        }
    }
}