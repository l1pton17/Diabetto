using System.Threading.Tasks;
using Diabetto.Core.ViewModels.Dialogs;

namespace Diabetto.Core.Services
{
    public interface IDialogService
    {
        void ShowAlert(string title, string message);

        Task<bool> ShowPicker(IDialogPickerViewModel source);
    }
}