using Diabetto.Core.ViewModels.Settings;
using ReactiveUI.Legacy;
#pragma warning disable 618

namespace Diabetto.Core.Services
{
    public interface ISettingViewModelsStorage
    {
        ReactiveList<ISettingsViewModel> Options { get; }
    }

    internal sealed class SettingViewModelsStorage : ISettingViewModelsStorage
    {
        /// <inheritdoc />
        public ReactiveList<ISettingsViewModel> Options { get; }

        public SettingViewModelsStorage()
        {
            Options = new ReactiveList<ISettingsViewModel>();
        }
    }
}