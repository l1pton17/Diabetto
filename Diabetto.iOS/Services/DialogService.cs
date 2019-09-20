using System.Threading.Tasks;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModels.Dialogs;
using Diabetto.iOS.Dialogs;
using Diabetto.iOS.ViewModels.Dialogs;

namespace Diabetto.iOS.Services
{
    public sealed class DialogService : IDialogService
    {
        public Task<bool> Show(IDialogPickerViewModel source)
        {
            var tcs = new TaskCompletionSource<bool>();
            var dialog = new PickerDialog<PickerDialogViewModel>();
            var pickerViewModel = new PickerDialogViewModel(dialog, source);

            dialog
                .Show(
                    source.Title,
                    callback: v => tcs.TrySetResult(true),
                    pickerViewModel,
                    cancelCallback: () => tcs.TrySetResult(false));

            return tcs.Task;
        }
    }
}