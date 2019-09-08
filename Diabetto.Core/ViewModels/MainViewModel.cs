using Diabetto.Core.ViewModels.Calendars;
using Diabetto.Core.ViewModels.Core;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace Diabetto.Core.ViewModels
{
    public class MainViewModel : MvxReactiveViewModel
    {
        public IMvxAsyncCommand ShowMeasuresViewModelCommand { get; }

        public MainViewModel(IMvxNavigationService navigationService)
        {
            ShowMeasuresViewModelCommand = new MvxAsyncCommand(() => navigationService.Navigate<CalendarViewModel>());
        }
    }
}