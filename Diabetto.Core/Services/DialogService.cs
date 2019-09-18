using System.Threading.Tasks;
using Diabetto.Core.ViewModels.Dialogs;

namespace Diabetto.Core.Services
{
    public interface IDialogService
    {
        Task<bool> Show(IDialogPickerViewModel source);
    }
}