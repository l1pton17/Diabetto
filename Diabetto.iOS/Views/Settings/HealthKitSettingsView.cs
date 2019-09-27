using Diabetto.iOS.ViewModels.Settings;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Diabetto.iOS.Views.Settings
{
    [MvxChildPresentation(Animated = true)]
    public sealed class HealthKitSettingsView : MvxTableViewController<HealthKitSettingsViewModel>
    {
        
    }
}