using System.Threading.Tasks;
using Diabetto.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Diabetto.Core
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return NavigationService.Navigate<MainViewModel>();
        }
    }
}