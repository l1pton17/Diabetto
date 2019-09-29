
using System;
using Diabetto.iOS.ViewModels.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Diabetto.iOS.Views.Settings
{
    [MvxFromStoryboard(StoryboardName = "HealthKitSettingsView")]
    [MvxChildPresentation(Animated = true)]
    public partial class HealthKitSettingsView : MvxTableViewController<HealthKitSettingsViewModel>
    {
        public HealthKitSettingsView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            var bindingSet = this.CreateBindingSet<HealthKitSettingsView, HealthKitSettingsViewModel>();

            bindingSet
                .Bind(EnabledSwitch)
                .For(v => v.Selected)
                .To(v => v.IsEnabled);

            bindingSet
                .Bind(ExportButton)
                .To(v => v.ExportCommand);

            bindingSet.Apply();
        }
    }
}