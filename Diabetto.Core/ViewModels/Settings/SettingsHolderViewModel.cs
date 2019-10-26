using System.Collections.Generic;
using System.Reactive;
using Diabetto.Core.Services;
using Diabetto.Core.ViewModels.Core;
using MvvmCross.Navigation;
using ReactiveUI;
#pragma warning disable 618

namespace Diabetto.Core.ViewModels.Settings
{
    public sealed class SettingsHolderViewModel : MvxReactiveViewModel
    {
        public IEnumerable<ISettingsViewModel> Options { get; }

        public ReactiveCommand<ISettingsViewModel, Unit> OptionSelectedCommand { get; }

        public SettingsHolderViewModel(
            IMvxNavigationService navigationService,
            ISettingViewModelsStorage settingViewModelsStorage)
        {
            Options = settingViewModelsStorage.Options;

            OptionSelectedCommand = ReactiveCommand.CreateFromTask<ISettingsViewModel>(
                v => navigationService.Navigate(v.GetType()));
        }
    }
}