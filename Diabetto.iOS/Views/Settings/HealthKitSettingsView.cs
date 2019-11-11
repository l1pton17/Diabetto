
using System;
using Diabetto.iOS.ViewModels.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Diabetto.iOS.Views.Settings
{
    [MvxFromStoryboard(StoryboardName = "HealthKitSettingsView")]
    [MvxModalPresentation(
        Animated = true,
        ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public partial class HealthKitSettingsView : MvxTableViewController<HealthKitSettingsViewModel>
    {
        public HealthKitSettingsView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var bindingSet = this.CreateBindingSet<HealthKitSettingsView, HealthKitSettingsViewModel>();
            
            bindingSet
                .Bind(EnabledSwitch)
                .To(v => v.IsEnabled);

            bindingSet
                .Bind(ExportButton)
                .To(v => v.ExportCommand);

            bindingSet
                .Bind(CloseButton)
                .To(v => v.CloseCommand);

            bindingSet.Apply();
        }
    }
}