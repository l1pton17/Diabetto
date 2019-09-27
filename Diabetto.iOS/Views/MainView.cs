using Diabetto.Core.ViewModels;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Diabetto.iOS.Views
{
    [MvxRootPresentation]
    public sealed class MainView : MvxTabBarViewController<MainViewModel>
    {
        private bool _firstTimePresented = true;

        public MainView()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowMeasuresViewModelCommand.Execute(null);
                ViewModel.ShowSettingsViewModelCommand.Execute(null);
            }
        }
    }
}