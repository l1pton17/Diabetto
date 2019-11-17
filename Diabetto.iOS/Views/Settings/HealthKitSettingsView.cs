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
        WrapInNavigationController = true,
        Animated = true,
        ModalPresentationStyle = UIModalPresentationStyle.FormSheet)]
    public partial class HealthKitSettingsView : MvxTableViewController<HealthKitSettingsViewModel>
    {
        public HealthKitSettingsView(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Health kit";
            ModalInPresentation = true;

            if (ParentViewController is MvxNavigationController parent)
            {
                parent.View.BackgroundColor = UIColor.SystemGray5Color;
            }

            TableView.ContentInset = new UIEdgeInsets(30, 0, 0, 0);

            NavigationItem.LeftBarButtonItem = new UIBarButtonItem
            {
                Title = "Close"
            };

            var bindingSet = this.CreateBindingSet<HealthKitSettingsView, HealthKitSettingsViewModel>();

            bindingSet
                .Bind(EnabledSwitch)
                .To(v => v.IsEnabled);

            bindingSet
                .Bind(ExportButton)
                .To(v => v.ExportCommand);

            bindingSet
                .Bind(NavigationItem.LeftBarButtonItem)
                .To(v => v.CloseCommand);

            bindingSet.Apply();
        }
    }
}