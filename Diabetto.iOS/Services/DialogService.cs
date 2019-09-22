using System.Threading.Tasks;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModels.Dialogs;
using Diabetto.iOS.Dialogs;
using Diabetto.iOS.ViewModels.Dialogs;
using UIKit;

namespace Diabetto.iOS.Services
{
    public sealed class DialogService : IDialogService
    {
        /// <inheritdoc />
        public void ShowAlert(string title, string message)
        {
            var alertView = new UIAlertView(
                title,
                message,
                null,
                "Ok");

            alertView.Show();
        }

        public Task<bool> ShowPicker(IDialogPickerViewModel source)
        {
            var tcs = new TaskCompletionSource<bool>();
            var dialog = new PickerDialog<PickerDialogViewModel>();
            var pickerViewModel = new PickerDialogViewModel(dialog, source);

            dialog
                .Show(
                    source.Title,
                    callback: v => tcs.TrySetResult(true),
                    pickerViewModel,
                    pickerCallback: v =>
                    {
                        foreach (var selectedItem in source.SelectedItems)
                        {
                            v.Select(selectedItem.Row, selectedItem.Component, animated: false);
                        }
                    },
                    cancelCallback: () => tcs.TrySetResult(false));

            return tcs.Task;
        }
    }
}