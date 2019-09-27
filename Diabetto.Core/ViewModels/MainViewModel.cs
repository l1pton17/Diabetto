using Diabetto.Core.ViewModels.Calendars;
using Diabetto.Core.ViewModels.Core;
using Diabetto.Core.ViewModels.Settings;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace Diabetto.Core.ViewModels
{
    public class MainViewModel : MvxReactiveViewModel
    {
        public IMvxAsyncCommand ShowMeasuresViewModelCommand { get; }

        public IMvxAsyncCommand ShowSettingsViewModelCommand { get; }

        public MainViewModel(IMvxNavigationService navigationService)
        {
            ShowSettingsViewModelCommand = new MvxAsyncCommand(() => navigationService.Navigate<SettingsHolderViewModel>());
            ShowMeasuresViewModelCommand = new MvxAsyncCommand(() => navigationService.Navigate<CalendarViewModel>());
        }
    }
}